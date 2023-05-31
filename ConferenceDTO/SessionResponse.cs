using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConferenceDTO;

public class SessionResponse : Session
{
    public Track Track { get; set; } = null!;

    public List<Speaker> Speakers { get; set; } = new List<Speaker>();
}