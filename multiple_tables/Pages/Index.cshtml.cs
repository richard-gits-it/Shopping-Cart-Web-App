﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using multiple_tables.Services;

namespace multiple_tables.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;


        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;

        }

        public void OnGet()
        {

        }
    }
}