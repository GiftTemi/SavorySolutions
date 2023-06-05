namespace SavourySolutions.Models.ViewModels.Chat
{
    using System;

    using SavourySolutions.Data.Models;
    using SavourySolutions.Services.Mapping;

    public class MessageViewModel : IMapFrom<Message>
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public string UserUsername { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
