using Microsoft.Extensions.Options;
using MongoDB.Driver;
using WebApiMongoDB.Model;

namespace WebApiMongoDB.Services
{
    public class ProdutoServices
    {
        private readonly IMongoCollection<Produto> _produtoCollection;

        public ProdutoServices(IOptions<ProdutoDatabaseSettings> produtoServices)
        {
            // Configurar MongoDb
            var mongoClient = new MongoClient(produtoServices.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(produtoServices.Value.DatabaseName);

            _produtoCollection = mongoDatabase.GetCollection<Produto>
                (produtoServices.Value.ProdutoCollectionName);
        }

        public async Task<List<Produto>> GetListAsync() => 
            await _produtoCollection.Find(x => true).ToListAsync();

        public async Task<Produto> GetAsync(string id) =>
            await _produtoCollection.Find(x => x.Id == id).FirstAsync();

        public async Task CreateAsync(Produto produto) =>
            await _produtoCollection.InsertOneAsync(produto);

        public async Task<Produto> UpdateAsync(string id, Produto produto)
        {
           await _produtoCollection.ReplaceOneAsync(x => x.Id == id, produto);
        
            return await GetAsync(id);
        }   

        public async Task RemoveAsync(string id) =>
           await _produtoCollection.DeleteOneAsync(x => x.Id == id);

    }
}
