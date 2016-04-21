using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Model
{
    [Serializable]
    class DbUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string DeviceId { get; set; }
        public string FacebookId { get; set; }
    }
}
