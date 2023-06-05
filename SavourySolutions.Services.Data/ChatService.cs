namespace SavourySolutions.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using SavourySolutions.Data.Common.Repositories;
    using SavourySolutions.Data.Models;
    using SavourySolutions.Models.ViewModels.Chat;
    using SavourySolutions.Services.Data.Common;
    using SavourySolutions.Services.Data.Contracts;
    using SavourySolutions.Services.Mapping;

    using Microsoft.EntityFrameworkCore;

    public class ChatService : IChatService
    {
        private readonly IDeletableEntityRepository<Message> messagesRepository;

        public ChatService(IDeletableEntityRepository<Message> messagesRepository)
        {
            this.messagesRepository = messagesRepository;
        }

        public async Task<MessageViewModel> CreateAsync(MessageInputModel messageCreateInputModel)
        {
            var message = new Message
            {
                UserId = messageCreateInputModel.UserId,
                UserName = messageCreateInputModel.UserName,
                Content = messageCreateInputModel.Content,
            };

            await this.messagesRepository.AddAsync(message);
            await this.messagesRepository.SaveChangesAsync();

            var viewModel = await this.GetViewModelByIdAsync<MessageViewModel>(message.Id);

            return viewModel;
        }

        public async Task DeleteByIdAsync(int id)
        {
            var message = await this.messagesRepository
                .All()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (message == null)
            {
                throw new NullReferenceException(
                    string.Format(ExceptionMessages.MessageNotFound, id));
            }

            this.messagesRepository.Delete(message);
            await this.messagesRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<TViewModel>> GetAllMessagesAsync<TViewModel>()
        {
            var messages = await this.messagesRepository
              .All()
              .To<TViewModel>()
              .ToListAsync();

            return messages;
        }

        public async Task<TViewModel> GetViewModelByIdAsync<TViewModel>(int id)
        {
            var messageViewModel = await this.messagesRepository
                .All()
                .Where(m => m.Id == id)
                .To<TViewModel>()
                .FirstOrDefaultAsync();

            if (messageViewModel == null)
            {
                throw new NullReferenceException(string.Format(ExceptionMessages.MessageNotFound, id));
            }

            return messageViewModel;
        }
    }
}
