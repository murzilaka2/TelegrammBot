using ConsoleApp1.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Repository
{
	public class NoteRepository
	{
		private readonly ApplicationContext _context;

		public NoteRepository(ApplicationContext context) => _context = context;

		public async Task AddNote(Note note)
		{
			_context.Add(note);
			await _context.SaveChangesAsync();
		}
		public async Task<Note?> GetById(int id)
		{
			var note = await _context.Notes.FirstOrDefaultAsync(n => n.Id == id);
			return note;
		}
		public async Task Delete(int id)
		{
			var note = await GetById(id);
			if (note == null) return;
			_context.Remove(note);
			await _context.SaveChangesAsync();
		}
		

	}
}
