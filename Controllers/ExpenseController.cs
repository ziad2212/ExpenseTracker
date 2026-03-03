using ExpenseTracker.Application.DTOs.Expense;
using ExpenseTracker.Application.Services.Expense;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExpenseTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ExpenseController : ControllerBase
    {
        private  readonly IExpenseService _expenseService;
        public ExpenseController(IExpenseService expenseService)
        {
            _expenseService = expenseService;
        }
        [HttpGet]
        public async Task<IActionResult> GetExpenses([FromQuery] string? filter,
        [FromQuery] DateTime? start,
        [FromQuery] DateTime? end)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var response = await _expenseService.GetExpenses(userId, filter, start, end);
                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); // 404
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message); // 400
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); // unexpected errors
            }


        }
         [HttpPost]
         public async Task<IActionResult> CreateExpense([FromBody] CreateExpenseRequest request)
         {
             try
             {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var response = await _expenseService.CreateExpense(userId, request);
                 return Ok(response);
             }
             catch (KeyNotFoundException ex)
             {
                 return NotFound(ex.Message); // 404
             }
             catch (ApplicationException ex)
             {
                 return BadRequest(ex.Message); // 400
             }
             catch (Exception ex)
             {
                 return StatusCode(500, ex.Message); // unexpected errors
             }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetExpenseById(Guid id)
        {
                try
                {
                    var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                    var response = await _expenseService.GetExpenseById(userId, id);
                    return Ok(response);
                }
                catch (KeyNotFoundException ex)
                {
                    return NotFound(ex.Message); // 404
                }
                catch (ApplicationException ex)
                {
                    return BadRequest(ex.Message); // 400
                }
                catch (Exception ex)
                {
                    return StatusCode(500, ex.Message); // unexpected errors
            }

        }
         [HttpPut("{id}")]
         public async Task<IActionResult> UpdateExpense( Guid id, [FromBody] UpdateExpenseRequest request)
         {
                try
                {
                    var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                    var response = await _expenseService.UpdateExpense(userId, id, request);
                    return Ok(response);
                }
                catch (KeyNotFoundException ex)
                {
                    return NotFound(ex.Message); // 404
                }
                catch (ApplicationException ex)
                {
                    return BadRequest(ex.Message); // 400
                }
                catch (Exception ex)
                {
                    return StatusCode(500, ex.Message); // unexpected errors
            }

        }
         [HttpDelete("{id}")]
         public async Task<IActionResult> DeleteExpense(Guid id)
         {
                try
                {
                    var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                    var success = await _expenseService.DeleteExpense(userId, id);
                    if (success)
                    {
                        return NoContent(); // 204
                    }
                    else
                    {
                        return NotFound("Expense not found."); // 404
                    }
                }
                catch (KeyNotFoundException ex)
                {
                    return NotFound(ex.Message); // 404
                }
                catch (ApplicationException ex)
                {
                    return BadRequest(ex.Message); // 400
                }
                catch (Exception ex)
                {
                    return StatusCode(500, ex.Message); // unexpected errors
            }
        }
    }
}
