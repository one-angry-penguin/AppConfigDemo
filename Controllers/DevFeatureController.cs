using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppConfigDemo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using Microsoft.FeatureManagement;

namespace AppConfigDemo.Controllers
{
    [FeatureGate(FeatureFlags.AllowDevFeature)]
    public class DevFeatureController : Controller
    {
        private readonly IFeatureManager _featureManager;

        public DevFeatureController(IFeatureManager featureManager)
        {
            _featureManager = featureManager;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}