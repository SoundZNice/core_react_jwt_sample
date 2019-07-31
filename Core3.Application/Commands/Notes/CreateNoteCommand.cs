using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Core3.Application.Commands.Notes
{
    public class CreateNoteCommand : IRequest
    {
        public string Text { get; set; }
    }
}
