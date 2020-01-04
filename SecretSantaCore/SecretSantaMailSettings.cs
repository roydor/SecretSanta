using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretSanta
{
    [Serializable]
    public class SecretSantaMailSettings
    {
        /// <summary>
        /// The gmail account to send mail from.
        /// </summary>
        public string SantasEmail;

        /// <summary>
        /// The password of the above account.
        /// </summary>
        public string SantasPassword;

        /// <summary>
        /// The message to be in the subject of the announcement mail.
        /// {0} -- Gifter's Name
        /// {1} -- Recipient's Name
        /// </summary>
        public string SantasSubject;

        /// <summary>
        /// The message to be in the body of the announcement mail.
        /// HTML is supported.
        /// {0} -- Gifter's Name
        /// {1} -- Recipient's Name
        /// </summary>
        public string SantasMessage;
    }
}
