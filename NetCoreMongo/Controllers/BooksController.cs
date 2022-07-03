using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetCoreMongo.Models;
using NetCoreMongo.Services;

namespace NetCoreMongo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            this._bookService = bookService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _bookService.GetAllAsync());
        }

        [HttpGet("{id:length(24)}")]
        public async Task<IActionResult> Get(string id)
        {
            var book = await _bookService.GetAsync(id);

            if (book == null)
                return NotFound("No Book found!");
            return Ok(book);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Book book)
        {
            if (book == null)
                throw new Exception("Invalid input!");

            await _bookService.CreateAsync(book);
            return CreatedAtAction(nameof(Get), new { id = book.Id }, book);
        }

        [HttpPut]
        public async Task<IActionResult> Update(string id, Book updateBook)
        {
            var book = await _bookService.GetAsync(id);

            if (book is null)
                return NotFound("No Book found!");

            updateBook.Id = book.Id;
            await _bookService.UpdateAsync(id, updateBook);
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var book = await _bookService.GetAsync(id);

            if (book is null)
            {
                return NotFound();
            }

            await _bookService.RemoveAsync(id);

            return NoContent();
        }
    }
}
