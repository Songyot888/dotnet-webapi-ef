using api_lotto.DTOs.auth;
using api_lotto.Helpers;
using api_lotto.Mappers;
using dotnet_webapi_ef.Data;
using lotto_api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api_lotto.controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly ApplicationDBContext _context;
        public AuthController(ApplicationDBContext context)
        {
            _context = context;
        }


        [HttpGet("user")]
        public async Task<IActionResult> Getall()
        {
            var users = await _context.Users
          .Select(u => new
          {
              u.Uid,
              u.FullName,
              u.Email,
              u.Phone,
              u.Balance,
              u.BankName,
              u.BankNumber,
          })
          .ToListAsync();

            return Ok(users);
        }

        [HttpGet("user/{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var users = _context.Users.Where(t => t.Uid == id).Select(u => new
            {
                u.Uid,
                u.FullName,
                u.Email,
                u.Phone,
                u.Balance,
                u.BankName,
                u.BankNumber,
                u.Role
            }).FirstOrDefault();

            if (users == null)
            {
                return NotFound();
            }

            return Ok(users);
        }

        [HttpPost("login")]
        public async Task<IActionResult> login([FromBody] LoginDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest(new { message = "กรุณากรอกข้อมูลให้ครบ" });

            var email = dto.Email.Trim().ToLowerInvariant();

            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null || !PasswordHelper.VerifyPassword(dto.Password, user.Password))
                return Unauthorized(new { message = "อีเมลหรือรหัสผ่านไม่ถูกต้อง" });
            return Ok(new
            {
                message = "เข้าสู่ระบบสำเร็จ",
                user = user.ToAuthLoginResponse()
            });
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
        {
            if (string.IsNullOrEmpty(dto.FullName) || string.IsNullOrEmpty(dto.Email) || string.IsNullOrEmpty(dto.Password))
            {
                return BadRequest(new { message = "กรุณากรอกข้อมูลให้ครบ" });
            }

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (existingUser != null)
            {
                return BadRequest(new { message = "อีเมลนี้ถูกใช้ไปแล้ว" });
            }

            var existingUserByPhone = await _context.Users.FirstOrDefaultAsync(u => u.Phone == dto.Phone);
            if (existingUserByPhone != null)
            {
                return BadRequest(new { message = "เบอร์โทรศัพท์นี้ถูกใช้ไปแล้ว" });
            }

            var existingUserByBankNumber = await _context.Users.FirstOrDefaultAsync(u => u.BankNumber == dto.BankNumber);
            if (existingUserByBankNumber != null)
            {
                return BadRequest(new { message = "เลขบัญชีนี้ถูกใช้ไปแล้ว" });
            }


            string hashedPassword = PasswordHelper.HashPassword(dto.Password);
            var user = dto.ToRegister(hashedPassword);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(user.ToAuthRegisterResponse());
        }


        // [HttpPost("request-otp")]
        // public async Task<IActionResult> RequestOtp([FromBody] string email, [FromServices] OtpService otpSvc, [FromServices] EmailService mail, [FromServices] IConfiguration cfg)
        // {
        //     if (string.IsNullOrEmpty(email)) return BadRequest(new { message = "กรุณากรอกอีเมล" });

        //     var otp = otpSvc.GenerateOtp(6);
        //     otpSvc.StoreOtp(email, otp, TimeSpan.FromMinutes(cfg.GetValue("Otp:TtlMinutes", 10)));
        //     await mail.SendOtpAsync(email, otp);
        //     return Ok(new { message = "ส่ง OTP ไปที่อีเมลแล้ว" });
        // }

        // [HttpPost("verify-otp")]
        // public IActionResult VerifyOtp([FromBody] dynamic body, [FromServices] OtpService otpSvc, [FromServices] IConfiguration cfg)
        // {
        //     string email = body?.email;
        //     string otp = body?.otp;
        //     if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(otp))
        //         return BadRequest(new { message = "ข้อมูลไม่ครบ" });

        //     var ok = otpSvc.ValidateOtp(email, otp, cfg.GetValue("Otp:MaxAttempts", 5));
        //     if (!ok) return BadRequest(new { message = "OTP ไม่ถูกต้องหรือหมดอายุ" });

        //     return Ok(new { message = "ยืนยัน OTP สำเร็จ" });
        // }
    }
}