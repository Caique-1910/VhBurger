using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VhBurger.Applications.Services;
using VhBurger.DTOs.CategoriaDTO;
using VhBurger.Exceptions;
using VhBurger.Interfaces;

namespace VhBurguer.Test.Services
{
    public class CategoriaServicesTests
    {
        [Fact]
        public void Adicionar_DeveGerarErro_QuandoEstiverVazio()
        {
            Mock<ICategoriaRepository> repositoryMock = new Mock<ICategoriaRepository>();

            CategoriaService service = new CategoriaService(repositoryMock.Object);

            CriarCategoriaDTO categoriaDto = new CriarCategoriaDTO
            {
                Nome = ""
            };

            Action acao = () => service.Adicionar(categoriaDto);

            acao.Should().Throw<DomainException>().WithMessage("Nome é obrigatório.");
        }

        [Fact]
        public void Adicionar_DeveGerarErro_QuandoCategoriaExistir()
        {
            Mock<ICategoriaRepository> repositoryMock = new Mock<ICategoriaRepository>();

            repositoryMock.Setup(repo => repo.NomeExiste("Lanche", It.IsAny<int?>())).Returns(true);

            CategoriaService service = new CategoriaService(repositoryMock.Object);

            CriarCategoriaDTO categoriaDto = new CriarCategoriaDTO
            {
                Nome = "Lanche"
            };

            Action acao = () => service.Adicionar(categoriaDto);

            acao.Should().Throw<DomainException>().WithMessage("Categoria já existente.");
        }
    }
}
