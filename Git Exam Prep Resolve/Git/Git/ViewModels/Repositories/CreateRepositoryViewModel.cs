﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git.ViewModels.Repositories
{
    public class CreateRepositoryViewModel
    {
        public string Name { get; set; }

        public string RepositoryType { get; set; }

        public string CreatorId { get; set; }
    }
}
