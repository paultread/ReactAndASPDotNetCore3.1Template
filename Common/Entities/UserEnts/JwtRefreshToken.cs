using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Common.Entities.UserEnts
{
    /// <summary>
    /// THis is not in app settings as it needs a CreaionDate prop, this is obviously dynamic and casnnot be set in app settings
    /// </summary>
    public class JwtRefreshToken
    {
        //tomorrow
        //create DTO for this
        //test interction with this entity
        //Find out why migration trie changing the names of Identity table - check migration added - commented otu code
        //add refresh token to samesite cookie only so it can be validated - never used in clicen t- not in local store etc
        [Key]
        //this attribute tag tells Entity framework to create the field so that the DB will generate this value and it will never be updated
        //suggested this doesnt work on strings although this is ancient?
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //this is important - this is what the front end uses for the refresh token
        public string RefreshTokenId { get; set; }
        public string JwtId { get; set; }
        public DateTime CreationDate { get; set; }
        public int ExpiresAfterSeconds { get; set; }
        public bool HasBeenUsedForARefresh { get; set; }
        //if user changes pasword or email, they are required to get a new token . refresh token
        public bool Invalidated { get; set; }

        //will be an email address
        public string UserId { get; set; }

        //navigational property - 
        //nameof equates to teh prop name.. best way to handle in case of prop changes
        //states th foreign key for this navigational property is this
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }
    }
}
