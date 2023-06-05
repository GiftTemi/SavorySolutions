
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using SavourySolutions.Data;
using SavourySolutions.Data.Models;
using SavourySolutions.Data.Models.Enumerations;
using SavourySolutions.Data.Repositories;
using SavourySolutions.Models.ViewModels.ArticleComments;
using SavourySolutions.Services.Data.Common;
using SavourySolutions.Services.Data.Contracts;
using SavourySolutions.Services.Mapping;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace SavourySolutions.Services.Data.Tests;
public class ArticleCommentsServiceTests : IAsyncDisposable
{
    private readonly IArticleCommentsService articleCommentsService;
    private EfDeletableEntityRepository<Article> articlesRepository;
    private EfDeletableEntityRepository<Category> categoriesRepository;
    private EfDeletableEntityRepository<ArticleComment> articleCommentsRepository;
    private EfDeletableEntityRepository<SavourySolutionsUser> usersRepository;
    private SqliteConnection connection;

    private Article firstArticle;
    private Category firstCategory;
    private ArticleComment firstArticleComment;
    private SavourySolutionsUser firstSavourySolutionsUser;

    public ArticleCommentsServiceTests()
    {
        this.InitializeMapper();
        this.InitializeDatabaseAndRepositories();
        this.InitializeFields();

        this.articleCommentsService = new ArticleCommentsService(this.articleCommentsRepository);
    }

    [Fact]
    public async Task CheckIfCreateAsyncWorksCorrectly()
    {
        this.SeedDatabase();

        var articleComment = new CreateArticleCommentInputModel
        {
            ArticleId = this.firstArticle.Id,
            Content = "I like this article.",
        };

        await this.articleCommentsService.CreateAsync(
            articleComment.ArticleId,
            this.firstSavourySolutionsUser.Id,
            articleComment.Content);

        var count = await this.articleCommentsRepository.All().CountAsync();

        Assert.Equal(1, count);
    }

    [Fact]
    public async Task CheckSettingOfArticleCommentProperties()
    {
        this.SeedDatabase();

        var model = new CreateArticleCommentInputModel
        {
            ArticleId = this.firstArticle.Id,
            Content = "What's your opinion for the article?",
        };

        await this.articleCommentsService.CreateAsync(model.ArticleId, this.firstSavourySolutionsUser.Id, model.Content);

        var articleComment = await this.articleCommentsRepository.All().FirstOrDefaultAsync();

        Assert.Equal(model.ArticleId, articleComment.ArticleId);
        Assert.Equal("What's your opinion for the article?", articleComment.Content);
    }

    [Fact]
    public async Task CheckIfAddingArticleCommentThrowsArgumentException()
    {
        this.SeedDatabase();
        await this.SeedArticleComments();

        var articleComment = new CreateArticleCommentInputModel
        {
            ArticleId = this.firstArticle.Id,
            Content = this.firstArticleComment.Content,
        };

        var exception = await Assert
            .ThrowsAsync<ArgumentException>(async ()
                => await this.articleCommentsService
                .CreateAsync(articleComment.ArticleId, this.firstSavourySolutionsUser.Id, articleComment.Content));

        Assert.Equal(
            string.Format(
                ExceptionMessages.ArticleCommentAlreadyExists, articleComment.ArticleId, articleComment.Content), exception.Message);
    }

    [Fact]
    public async Task CheckIfIsInArticleIdReturnsTrue()
    {
        this.SeedDatabase();
        await this.SeedArticleComments();

        var articleCommentId = await this.articleCommentsRepository
            .All()
            .Select(x => x.ArticleId)
            .FirstOrDefaultAsync();

        var result = await this.articleCommentsService.IsInArticleId(articleCommentId, this.firstArticle.Id);

        Assert.True(result);
    }

    [Fact]
    public async Task CheckIfIsInArticleIdReturnsFalse()
    {
        this.SeedDatabase();
        await this.SeedArticleComments();

        var result = await this.articleCommentsService.IsInArticleId(3, this.firstArticle.Id);

        Assert.False(result);
    }

    public async ValueTask DisposeAsync()
    {
        await this.connection.CloseAsync();
        await this.connection.DisposeAsync();
    }

    private void InitializeDatabaseAndRepositories()
    {
        this.connection = new SqliteConnection("DataSource=:memory:");
        this.connection.Open();
        var options = new DbContextOptionsBuilder<SavourySolutionsDbContext>().UseSqlite(this.connection);
        var dbContext = new SavourySolutionsDbContext(options.Options);

        dbContext.Database.EnsureCreated();

        this.usersRepository = new EfDeletableEntityRepository<SavourySolutionsUser>(dbContext);
        this.articlesRepository = new EfDeletableEntityRepository<Article>(dbContext);
        this.articleCommentsRepository = new EfDeletableEntityRepository<ArticleComment>(dbContext);
        this.categoriesRepository = new EfDeletableEntityRepository<Category>(dbContext);
    }

    private void InitializeFields()
    {
        this.firstSavourySolutionsUser = new SavourySolutionsUser
        {
            Id = "1",
            FullName = "Stamat Stamatov",
            UserName = "Stamat99",
            Gender = Gender.Male,
        };

        this.firstCategory = new Category
        {
            Name = "Vegetables",
            Description = "Test category description",
        };

        this.firstArticle = new Article
        {
            Id = 1,
            Title = "Test article title",
            Description = "Test article description",
            ImagePath = "https://someimageurl.com",
            CategoryId = 1,
            UserId = "1",
        };

        this.firstArticleComment = new ArticleComment
        {
            ArticleId = this.firstArticle.Id,
            Content = "Nice article.",
            UserId = this.firstSavourySolutionsUser.Id,
        };
    }

    private async void SeedDatabase()
    {
        await this.SeedUsers();
        await this.SeedCategories();
        await this.SeedArticles();
    }

    private async Task SeedUsers()
    {
        await this.usersRepository.AddAsync(this.firstSavourySolutionsUser);
        await this.usersRepository.SaveChangesAsync();
    }

    private async Task SeedCategories()
    {
        await this.categoriesRepository.AddAsync(this.firstCategory);
        await this.categoriesRepository.SaveChangesAsync();
    }

    private async Task SeedArticles()
    {
        await this.articlesRepository.AddAsync(this.firstArticle);
        await this.articlesRepository.SaveChangesAsync();
    }

    private async Task SeedArticleComments()
    {
        await this.articleCommentsRepository.AddAsync(this.firstArticleComment);
        await this.articleCommentsRepository.SaveChangesAsync();
    }

    private void InitializeMapper() => AutoMapperConfig.
        RegisterMappings(Assembly.Load("SavourySolutions.Models.ViewModels"));
}
