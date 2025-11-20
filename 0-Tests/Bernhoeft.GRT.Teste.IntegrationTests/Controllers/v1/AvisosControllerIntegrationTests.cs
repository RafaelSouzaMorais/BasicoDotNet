using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Bernhoeft.GRT.Teste.IntegrationTests.Controllers.v1
{
    public class AvisosControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public AvisosControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }
        #region Tests CreateAviso
        [Fact]
        public async Task CreateAviso_DeveRetornarCreated_QuandoDadosValidos()
        {
            // Arrange
            var payload = new { Titulo = "Aviso Teste", Mensagem = "Mensagem Teste" };
            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/v1/avisos", content);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var body = await response.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(body).RootElement.GetProperty("Dados");
            Assert.True(doc.GetProperty("Id").GetInt32() > 0);
            Assert.True(doc.GetProperty("Ativo").GetBoolean());
            Assert.Equal(payload.Titulo, doc.GetProperty("Titulo").GetString());
            Assert.Equal(payload.Mensagem, doc.GetProperty("Mensagem").GetString());
        }

        [Fact]
        public async Task CreateAviso_DeveRetornarBadRequest_QuandoDadosInvalidos()
        {
            // Arrange
            var payload = new { Titulo = "", Mensagem = "" }; // Dados inválidos
            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            // Act
            var response = await _client.PostAsync("/api/v1/avisos", content);
            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task CreateAviso_DeveRetornarBadRequest_QuandoMensagemExcedeTamanho()
        {
            // Arrange
            var payload = new { Titulo = "Aviso Teste", Mensagem = new string('A', 1001) }; // Mensagem com 1001 caracteres
            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/v1/avisos", content);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        #endregion

        #region Tests GetAvisos
        [Fact]
        public async Task GetAvisos_DeveRetornarOk_QuandoExistemAvisosAtivos()
        {
            // Act
            var response = await _client.GetAsync("/api/v1/avisos");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var body = await response.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(body).RootElement.GetProperty("Dados");
            Assert.True(doc.GetArrayLength() > 0);
        }

        [Fact]
        public async Task GetAvisoById_DeveRetornarOk_QuandoAvisoExiste()
        {
            // Arrange
            var avisoId = 1; // Certifique-se de que este ID existe no banco de dados de teste
            // Act
            var response = await _client.GetAsync($"/api/v1/avisos/{avisoId}");
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var body = await response.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(body).RootElement.GetProperty("Dados");
            Assert.Equal(doc.GetProperty("Id").GetInt32(), avisoId);
        }

        [Fact]
        public async Task GetAvisoById_DeveRetornarNotFound_QuandoAvisoNaoExiste()
        {
            // Arrange
            var avisoId = 9999; // ID que não existe
            // Act
            var response = await _client.GetAsync($"/api/v1/avisos/{avisoId}");
            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
        #endregion

        #region Tests UpdateAviso
        [Fact]
        public async Task UpdateAviso_DeveRetornarOk_QuandoDadosValidos()
        {
            // Arrange
            var payload = new { Mensagem = "Mensagem Teste" };
            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync("/api/v1/avisos/1", content);

            // Assert            
            var body = await response.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(body).RootElement.GetProperty("Dados");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(payload.Mensagem, doc.GetProperty("Mensagem").GetString());
        }

        [Fact]
        public async Task UpdateAviso_DeveRetornarOK_NaoDeveModificarTitulo()
        {
            //Não deve permitir atualizar o titulo
            // Arrange
            var payload = new { Titulo = "Aviso Teste", Mensagem = "Mensagem Teste" };
            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync("/api/v1/avisos/1", content);

            // Assert
            var body = await response.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(body).RootElement.GetProperty("Dados");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotEqual(payload.Titulo, doc.GetProperty("Titulo").GetString());

        }
        [Fact]
        public async Task UpdateAviso_DeveRetornarNotFound_QuandoAvisoNaoExiste()
        {
            // Arrange
            var payload = new { Mensagem = "Mensagem Teste" };
            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync("/api/v1/avisos/9999", content);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task UpdateAviso_DeveRetornarBadRequest_QuandoMensagemVazia()
        {
            // Arrange
            var payload = new { Mensagem = "" };
            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync("/api/v1/avisos/1", content);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task UpdateAviso_DeveRetornarBadRequest_QuandoMensagemExcedeTamanho()
        {
            // Arrange
            var payload = new { Mensagem = new string('A', 1001) };
            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync("/api/v1/avisos/1", content);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task UpdateAviso_DeveRetornarBadRequest_QuandoDadosInvalidos()
        {
            // Arrange
            var payload = new { Mensagem = (string?)null }; // Dados inválidos
            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync("/api/v1/avisos/1", content);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task UpdateAviso_DeveRetornarNotFound_QuandoAvisoInativo()
        {
            // Arrange
            var payload = new { Mensagem = "Mensagem Teste" };
            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync("/api/v1/avisos/3", content);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
        #endregion

        #region Tests DeleteAviso
        [Fact]
        public async Task DeleteAviso_DeveRetornarNoContent_QuandoAvisoExiste()
        {
            // Act
            var response = await _client.DeleteAsync("/api/v1/avisos/2");
            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task DeleteAviso_DeveRetornarNotFound_QuandoAvisoNaoExiste()
        {
            // Act
            var response = await _client.DeleteAsync("/api/v1/avisos/9999");
            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task DeleteAviso_DeveRetornarNotFound_QuandoAvisoJaFoiInativado()
        {
            // Act
            var response = await _client.DeleteAsync("/api/v1/avisos/3");
            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        #endregion

    }
}
