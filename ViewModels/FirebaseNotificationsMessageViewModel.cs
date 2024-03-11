﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _AbsoPickUp.ViewModels
{
    public class FirebaseNotificationsMessageViewModel
    {
        public string[] registration_ids { get; set; }
        public FirebaseNotificationsViewModel notification { get; set; }
        public object data { get; set; }
    }
    public class FirebaseNotificationsViewModel
    {
        public string title { get; set; }
        public string body { get; set; }
    }
}
