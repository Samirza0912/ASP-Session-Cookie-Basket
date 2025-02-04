﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Friello.ViewModels
{
    public class PaginationVM<T>
    {
        public List<T> Items { get; set; }
        public int PageCount { get; set; }
        public int CurrentPage { get; set; }

        public PaginationVM(List<T> items, int pageCount, int currentPage)
        {
            Items = items;
            PageCount = pageCount;
            CurrentPage = currentPage;
        }
    }
}
