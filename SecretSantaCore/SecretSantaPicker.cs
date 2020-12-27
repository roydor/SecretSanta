using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SecretSanta
{
    public class SecretSantaPicker
    {
        public int Attempts;
        public List<SecretSantaParticipant> Participants;

        public SecretSantaPicker(string participantsPath)
        {
            if (!File.Exists(participantsPath))
            {
                throw new FileNotFoundException(participantsPath);
            }

            // Read participants data from disk.
            string participantsJson = File.ReadAllText(participantsPath);
            List<SecretSantaParticipant> participants = JsonConvert.DeserializeObject<List<SecretSantaParticipant>>(participantsJson);

            // Ensure that there are no invalid restrictions
            bool validInput = CheckRestrictions(participants);
            if (!validInput)
            {
                throw new InvalidDataException();
            }

            Participants = participants;
        }

        public Dictionary<SecretSantaParticipant, SecretSantaParticipant> DrawNames()
        {
            if (Participants == null || !Participants.Any())
            {
                throw new InvalidDataException("No participants loaded, cannot draw names.");
            }

            Dictionary<SecretSantaParticipant, SecretSantaParticipant> results = null;
            do
            {
                Console.Clear();
                results = SelectAllSantas(Participants.ToList());
                Attempts++;
            } while (results == null);

            return results;
        } 

        /// <summary>
        /// Super bad and not very well thought through algorithm
        /// First calculate everyone that can a participant can give a gift to.
        /// Who ever has the least options, gets to choose first.
        
        /// </summary>
        /// <param name="localParticipants"></param>
        /// <returns></returns>
        private Dictionary<SecretSantaParticipant, SecretSantaParticipant> SelectAllSantas(List<SecretSantaParticipant> localParticipants)
        {
            Dictionary<SecretSantaParticipant, SecretSantaParticipant> results = new Dictionary<SecretSantaParticipant, SecretSantaParticipant>();
            List<SecretSantaParticipant> validRecipients = new List<SecretSantaParticipant>(localParticipants);

            // Establish a graph where vertices are participents
            // and edges represent who they can gift to.
            Dictionary<SecretSantaParticipant, List<SecretSantaParticipant>> selectionGraph = new Dictionary<SecretSantaParticipant, List<SecretSantaParticipant>>();
            foreach (var p in localParticipants)
            {
                selectionGraph[p] = new List<SecretSantaParticipant>();
                foreach (var recipient in validRecipients.Where(x => x != p && !p.Restrictions.Contains(x.Name)))
                {
                    selectionGraph[p].Add(recipient);
                }
            }

            while (selectionGraph.Keys.Count > 0)
            {
                // Deal with the participant who has the least options first.
                var participantEntry = selectionGraph.OrderBy(x => x.Value.Count).FirstOrDefault();
                participantEntry.Value.Shuffle();

                var recipient = participantEntry.Value.FirstOrDefault();
                if (recipient == null)
                {
                    return null;
                }

                // Record their choice
                results[participantEntry.Key] = recipient;
                foreach (var allRecipients in selectionGraph.Values)
                {
                    allRecipients.Remove(recipient);
                }

                validRecipients.Remove(recipient);
                selectionGraph.Remove(participantEntry.Key);
            }

            return results;
        }

        /// <summary>
        /// Runs over a list of participants ensuring that all the restrictions are valid.
        /// </summary>
        /// <param name="participants"></param>
        /// <returns></returns>
        public bool CheckRestrictions(List<SecretSantaParticipant> participants)
        {
            foreach (var participant in participants)
            {
                if (participant.Restrictions != null && participant.Restrictions.Any())
                {
                    foreach (var restriction in participant.Restrictions)
                    {
                        var exists = participants.Where(x => x.Name == restriction).FirstOrDefault();
                        if (exists == null)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }
    }
}
