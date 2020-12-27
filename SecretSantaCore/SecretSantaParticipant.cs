using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretSanta
{
    /// <summary>
    /// A class reprsenting a participant in a Secret Santa draw.
    /// </summary>
    [Serializable]
    public class SecretSantaParticipant
    {
        /// <summary>
        /// Name of this participant.
        /// </summary>
        public string Name;

        /// <summary>
        /// The email of this participant.
        /// </summary>
        public string Email;

        /// <summary>
        /// The names of participants that this person can not gift to.
        /// (Significant others / haters)
        /// </summary>
        public List<string> Restrictions;
    }
}
