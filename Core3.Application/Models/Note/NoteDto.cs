using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;

namespace Core3.Application.Models.Note
{
    public class NoteDto
    {
        public Guid Id { get; set; }

        public string Text { get; set; }
    }
}
