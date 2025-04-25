using Microsoft.AspNetCore.Mvc;
using SlotMachine.Models;
using SlotMachine.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MIDTERM_A1_SECTION_LASTNAME_FIRSTNAME.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SlotMachineController : ControllerBase
    {
        private static readonly List<Player> _players = new();
        private static readonly List<GameResult> _gameResults = new();
        private static readonly Random _random = new();

        [HttpGet("validate-player")]
        public IActionResult ValidatePlayer(string studentNumber)
        {
            if (string.IsNullOrWhiteSpace(studentNumber))
                return BadRequest("Student number is required.");

            if (!studentNumber.StartsWith("C") || !studentNumber.Substring(1).All(char.IsDigit))
                return BadRequest("Invalid student number format. Must start with 'C' followed by numbers.");

            var player = _players.FirstOrDefault(p => p.StudentNumber == studentNumber);
            if (player == null)
                return NotFound("Student number not found.");

            if (player.LastPlayed.AddHours(3) > DateTime.Now)
                return BadRequest($"Wait until {player.LastPlayed.AddHours(3)} to play again.");

            return Ok(new
            {
                studentNumber = player.StudentNumber,
                fullName = $"{player.FirstName} {player.LastName}"
            });
        }

        [HttpPost("save-game")]
        public IActionResult SaveGame([FromBody] GameResultRequest request)
        {
            var player = _players.FirstOrDefault(p => p.StudentNumber == request.StudentNumber);
            if (player == null) return NotFound("Player not found.");

            player.LastPlayed = DateTime.Now;

            var gameResult = new GameResult
            {
                StudentNumber = request.StudentNumber,
                StudentName = $"{player.FirstName} {player.LastName}",
                IsWin = request.IsWin,
                Retries = request.Retries,
                DatePlayed = DateTime.Now
            };

            _gameResults.Add(gameResult);
            return Ok(new { message = "Game saved successfully." });
        }

        // Add all other endpoints from previous implementation
        // [HttpGet("games")], [HttpGet("winners")], etc.

        public static void AddTestData()
        {
            if (_players.Any()) return;

            _players.AddRange(new List<Player>
            {
                new() { StudentNumber = "C12345678", FirstName = "John", LastName = "Doe" },
                new() { StudentNumber = "C87654321", FirstName = "Jane", LastName = "Smith" }
            });

            _gameResults.AddRange(new List<GameResult>
            {
                new() {
                    StudentNumber = "C12345678",
                    StudentName = "John Doe",
                    IsWin = true,
                    Retries = 3,
                    DatePlayed = DateTime.Now.AddDays(-1)
                },
                new() {
                    StudentNumber = "C87654321",
                    StudentName = "Jane Smith",
                    IsWin = false,
                    Retries = 0,
                    DatePlayed = DateTime.Now.AddHours(-1)
                }
            });
        }
    }
}