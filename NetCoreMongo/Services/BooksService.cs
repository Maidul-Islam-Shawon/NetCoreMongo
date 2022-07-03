using Microsoft.Extensions.Options;
using MongoDB.Driver;
using NetCoreMongo.Models;

namespace NetCoreMongo.Services
{
    public class BooksService : IBookService
    {
        private readonly IMongoCollection<Book> _books;
        public BooksService(IOptions<BookStoreDatabaseSettings> bookStoreDatabaseSettings)
        {
            var mongoClient = new MongoClient(bookStoreDatabaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(bookStoreDatabaseSettings.Value.DatabaseName);
            _books = mongoDatabase.GetCollection<Book>(bookStoreDatabaseSettings.Value.BooksCollectionName);
        }

        public async Task CreateAsync(Book newBook) =>
            await _books.InsertOneAsync(newBook);


        public async Task<List<Book>> GetAllAsync() =>
            await _books.Find(_ => true).ToListAsync();


        public async Task<Book?> GetAsync(string id) =>
              await _books.Find(x => x.Id == id).FirstOrDefaultAsync();


        public async Task RemoveAsync(string id) =>
            await _books.DeleteOneAsync(x => x.Id == id);


        public async Task UpdateAsync(string id, Book updatedBook) =>
            await _books.ReplaceOneAsync(x => x.Id == id, updatedBook);
    }
}
