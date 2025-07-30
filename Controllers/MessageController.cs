using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using EgitimPlatformu.Data;
using EgitimPlatformu.Models;

namespace EgitimPlatformu.Controllers
{
    [Authorize]
    public class MessageController : Controller
    {
        private readonly AppDbContext _context;

        public MessageController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Send([FromBody] SendMessageRequest request)
        {
            try
            {
                var senderId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                
                if (senderId == 0)
                {
                    return Json(new { success = false, message = "Kullanıcı bulunamadı" });
                }

                var message = new Message
                {
                    SenderId = senderId,
                    ReceiverId = request.ReceiverId,
                    Content = $"{request.Subject}: {request.Content}",
                    SentAt = DateTime.UtcNow,
                    IsRead = false
                };

                _context.Messages.Add(message);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Mesaj başarıyla gönderildi" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetMessages()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                
                var messages = await _context.Messages
                    .Where(m => m.ReceiverId == userId || m.SenderId == userId)
                    .Include(m => m.Sender)
                    .Include(m => m.Receiver)
                    .OrderByDescending(m => m.SentAt)
                    .Take(10)
                    .Select(m => new
                    {
                        id = m.Id,
                        senderId = m.SenderId,
                        receiverId = m.ReceiverId,
                        senderName = m.Sender.FirstName + " " + m.Sender.LastName,
                        receiverName = m.Receiver.FirstName + " " + m.Receiver.LastName,
                        content = m.Content,
                        sentAt = m.SentAt.ToString("dd.MM.yyyy HH:mm"),
                        isRead = m.IsRead,
                        isSentByMe = m.SenderId == userId
                    })
                    .ToListAsync();

                return Json(new { success = true, data = messages });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetChatMessages(int studentId)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                
                var messages = await _context.Messages
                    .Where(m => (m.SenderId == userId && m.ReceiverId == studentId) || 
                               (m.SenderId == studentId && m.ReceiverId == userId))
                    .Include(m => m.Sender)
                    .Include(m => m.Receiver)
                    .OrderBy(m => m.SentAt) // Sohbet için tarih sırasına göre (eskiden yeniye)
                    .Select(m => new
                    {
                        id = m.Id,
                        senderId = m.SenderId,
                        receiverId = m.ReceiverId,
                        senderName = m.Sender.FirstName + " " + m.Sender.LastName,
                        receiverName = m.Receiver.FirstName + " " + m.Receiver.LastName,
                        content = m.Content,
                        sentAt = m.SentAt.ToString("dd.MM.yyyy HH:mm"),
                        isRead = m.IsRead,
                        isSentByMe = m.SenderId == userId
                    })
                    .ToListAsync();

                // Bu sohbetteki okunmamış mesajları okundu olarak işaretle
                var unreadMessages = await _context.Messages
                    .Where(m => m.SenderId == studentId && m.ReceiverId == userId && !m.IsRead)
                    .ToListAsync();

                foreach (var message in unreadMessages)
                {
                    message.IsRead = true;
                    message.ReadAt = DateTime.UtcNow;
                }

                if (unreadMessages.Any())
                {
                    await _context.SaveChangesAsync();
                }

                return Json(new { success = true, data = messages });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> MarkAsRead(int messageId)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                
                var message = await _context.Messages
                    .FirstOrDefaultAsync(m => m.Id == messageId && m.ReceiverId == userId);

                if (message != null)
                {
                    message.IsRead = true;
                    message.ReadAt = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }

    public class SendMessageRequest
    {
        public string Subject { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int ReceiverId { get; set; }
    }
}