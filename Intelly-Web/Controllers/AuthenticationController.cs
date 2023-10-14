﻿using Intelly_Web.Entities;
using Intelly_Web.Interfaces;
using Intelly_Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Intelly_Web.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly IUserModel _userModel;

        public AuthenticationController(ILogger<AuthenticationController> logger, IUserModel userModel)
        {
            _logger = logger;
            _userModel = userModel;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult EmailSent()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserEnt entity)
        {
            var resp = await _userModel.Login(entity);

            if (resp.Success)
                return RedirectToAction("Index", "Home");
            else
            {
                ViewBag.MensajePantalla = resp.ErrorMessage;
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> PwdRecovery(UserEnt entity)
        {
            var resp = await _userModel.PwdRecovery(entity);

            if (resp.Success)
                return RedirectToAction("EmailSent", "Authentication");
            else
            {
                ViewBag.MensajePantalla = resp.ErrorMessage;
                return View();
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
