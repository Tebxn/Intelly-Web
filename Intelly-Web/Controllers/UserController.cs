﻿using Intelly_Web.Entities;
using Intelly_Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Intelly_Web.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserModel _userModel;


        //Inyeccion de dependencias, para que por medio de la interfaz el controller vea los metodos
        public UserController(ILogger<UserController> logger, IUserModel userModel)
        {
            _logger = logger;
            _userModel = userModel;
        }
       


        [HttpGet]
        public IActionResult AddUser()
        {
            return View();
        }


        [HttpPost]
        public IActionResult AddUser(UserEnt entity)
        {
            var resp = _userModel.AddUser(entity);
            if (resp == 1)
            {
                return RedirectToAction("NewEmployee", "AddUser");
            }
            else
            {
                ViewBag.MensajePantalla = "No se realizaron cambios";
                return View();

            }
        }

        

        [HttpPost]
        public IActionResult Login(UserEnt entity)
        {
            var resp = _userModel.Login(entity);

            if (resp != null)
                return RedirectToAction("NewSession", "Login");
            else
            {
                ViewBag.MensajePantalla = "No se pudo iniciar sesión";
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
