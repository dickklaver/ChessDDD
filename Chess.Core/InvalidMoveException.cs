﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Core
{
    public class InvalidMoveException: Exception
    {
        public InvalidMoveException(string message): base(message)
        {
        }
    }
}
