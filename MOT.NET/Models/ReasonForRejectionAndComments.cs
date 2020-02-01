namespace MOT.NET.Models {
    /// <summary>
    /// Represents a reason for rejection / test comment.
    /// </summary>
    public class ReasonForRejectionAndComments {
        /// <summary>
        /// Gets the text describing the reason for rejection / test comment.
        /// </summary>
        /// <value>The text describing the reason for rejection / test comment.</value>
        public string Text { get; set; }

        /// <summary>
        /// Gets the type of reason for rejection / test comment.
        /// </summary>
        /// <remarks>
        /// Can be FAIL, ADVISORY, MAJOR, DANGEROUS or USER ENTERED.
        /// </remarks>
        /// <value>The type of reason for rejection / test comment.</value>
        public string Type { get; set; }

        /// <summary>
        /// Whether or not the reason for rejection / test comment describes a dangerous defect.
        /// </summary>
        /// <value>TRUE if the reason for rejection / test comment describes a dangerous defect, FALSE otherwise.</value>
        public bool Dangerous { get; set; }
    }
}