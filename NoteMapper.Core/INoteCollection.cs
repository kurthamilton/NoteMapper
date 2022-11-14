﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteMapper.Core
{
    public interface INoteCollection : IReadOnlyCollection<Note>
    {
        bool Contains(Note note);
    }
}
