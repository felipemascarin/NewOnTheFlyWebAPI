﻿using DomainAPI.Dto.Company;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainAPI.Dto.Passenger {
    public class PassengerUpdateDTO {
        [StringLength(30)]
        public string Name { get; set; }
        [StringLength(1)]
        public string Gender { get; set; }
        [StringLength(14)]
        public string Phone { get; set; }
        public bool Status { get; set; }

        public PassengerAddressDTO Address { get; set; }
    }
}
