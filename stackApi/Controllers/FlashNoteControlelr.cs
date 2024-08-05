using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using stackApi.Data;
using stackApi.Model.Entities;

namespace stackApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")] //This makes it so that it routes to api/ then to the name of the controller
    public class FlashNoteController : Controller
    {

        private readonly FlashDbContext flash_db_context;
        public FlashNoteController(FlashDbContext flash_db_context)
        {
            this.flash_db_context = flash_db_context;
        }


        [HttpGet] // HttpGet, part of the CRUD accronym
        public async Task<IActionResult> GetAllNotes()
        {
            // With this method, you are getting the notes from database
            return Ok(await flash_db_context.Flash.ToListAsync());
        }

        [HttpGet]
        [Route("{Id:Guid}")]
        public async Task<IActionResult> GetDataById([FromRoute] Guid id)
        {
            var fetchData = await flash_db_context.Flash.FirstOrDefaultAsync(f => f.Id == id);

            if (fetchData == null)
            {
                return NotFound();
            }
            return Ok(fetchData);
        }

        [HttpPost]
        public async Task<IActionResult> AddFlashNotes(Flashcard flash_notes)
        {
            flash_notes.Id = Guid.NewGuid();
            await flash_db_context.Flash.AddAsync(flash_notes);
            await flash_db_context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetDataById), new { id = flash_notes.Id }, flash_notes);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateFlashCard([FromRoute] Guid id, [FromBody] Flashcard update_flash_notes)
        {

            var putNote = await flash_db_context.Flash.FirstOrDefaultAsync((fn) => fn.Id == id);
            if (putNote == null)
            {
                return NotFound();
            }
            putNote.Question = update_flash_notes.Question;
            putNote.Answer = update_flash_notes.Answer;
            putNote.IsVisible = update_flash_notes.IsVisible;
            putNote.CorrectAnswer = update_flash_notes.CorrectAnswer;
            putNote.IncorrectAnswer = update_flash_notes.IncorrectAnswer;
           

            await flash_db_context.SaveChangesAsync();

            return Ok(putNote);
        }

        [HttpDelete]
        [Route("{Id:Guid}")]
        public async Task<IActionResult> DeleteSubject([FromRoute] Guid id)
        {
            var deleteSub = await flash_db_context.Flash.FirstOrDefaultAsync((db) => db.Id == id);
            if(deleteSub == null)
            { 
                return NotFound(); 
            }

            flash_db_context.Remove(deleteSub);
            await flash_db_context.SaveChangesAsync();
            return Ok();

        }



    }
}
