using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtificialIntelligenceGraphChallenge
{
    internal class Status
    {
        // Iepriekšējais stāvoklis grafam.
        private Status? previousStatus;
        // Nākamais stāvoklis grafam.
        private Status? nextStatus;

        // Lāpas degšanas laiks, kas tika izmantots.
        private int timeSpent = 0;
        // Simbolu virkne, kas skaidro, kāds ir stāvoklis.
        private string statusInfo = string.Empty;

        // Saraksts, kurš glab ceļotājus, kuri atrodas stāvokļa P1.
        public List<Adventurer> adventurersWaiting = new List<Adventurer>();
        // Sarakss, kurš glab ceļotājus, kuri atrodas stāvokļa P2.
        public List<Adventurer> adventurersCrossed = new List<Adventurer>();

        /// <summary>
        /// Constructor for the Status class.
        /// </summary>
        public Status()
        {

        }

        /// <summary>
        /// Constructor for the Status class, that takes existing Status as a parameter.
        /// </summary>
        /// <param name="status">Status that exists and would be used to get values for the new one.</param>
        public Status(Status? status)
        {
            status.nextStatus = this;
            this.previousStatus = status;
            this.timeSpent = status.timeSpent;
            this.adventurersWaiting = new List<Adventurer>(status.adventurersWaiting);
            this.adventurersCrossed = new List<Adventurer>(status.adventurersCrossed);
        }

        public void SetTimeLeft(int time)
        {
            timeSpent = time;
            UpdateStatusInfo();
        }

        public int GetTimeSpent()
        {
            return timeSpent;
        }

        public string GetNextStatus()
        {
            return nextStatus.GetStatusInfo();
        }

        public void SetNextStatus(Status status)
        {
            this.nextStatus = status;
        }

        private void UpdateStatusInfo()
        {
            string adventuresWaitingIDs = String.Empty;
            string adventurersCrossedIDs = String.Empty;


            foreach(Adventurer adv in adventurersWaiting)
            {
                adventuresWaitingIDs += adv.GetIdentificationLetter();
            }

            foreach (Adventurer adv in adventurersCrossed)
            {
                adventurersCrossedIDs += adv.GetIdentificationLetter();
            }

            adventuresWaitingIDs = String.Concat(adventuresWaitingIDs.OrderBy(ch => ch));
            adventurersCrossedIDs = String.Concat(adventurersCrossedIDs.OrderBy(ch => ch));

            this.statusInfo = String.Format("{0};{1};{2}", timeSpent, adventuresWaitingIDs, adventurersCrossedIDs);
        }

        public string GetStatusInfo()
        {
            return statusInfo;
        }
    }
}
