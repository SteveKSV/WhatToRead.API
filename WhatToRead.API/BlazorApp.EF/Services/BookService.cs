using AutoMapper;
using BlazorApp.EF.Models;
using EFWhatToRead_BBL.Dtos;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using WhatToRead.API.AdoNet.BBL.Dtos;

namespace BlazorApp.EF.Services
{
    public class BookService
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;
        public BookService(HttpClient httpClient, IMapper mapper)
        {
            _httpClient = httpClient;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BookByAuthorDTO>> GetAllBooksAsync()
        {
            var url = _httpClient.BaseAddress;
            var response = await _httpClient.GetAsync($"{url}/Book/GetBooksWithAuthor");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<BookByAuthorDTO>>(json);
        }

        public async Task<Book> GetBookByIdAsync(int bookId)
        {
            var url = _httpClient.BaseAddress;
            var response = await _httpClient.GetAsync($"{url}/Book/{bookId}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            return _mapper.Map<BookDTO, Book>(JsonConvert.DeserializeObject<BookDTO>(json)); 
        }

        public async Task CreateBookAsync(Book book)
        {
            var bookToSerialise = _mapper.Map<Book, BookDTO>(book);
            var json = JsonConvert.SerializeObject(bookToSerialise);
            var bookToSend = new StringContent(json, Encoding.UTF8, "application/json");
            var url = _httpClient.BaseAddress;
            await _httpClient.PostAsync($"{url}/Book", bookToSend);
        }

        public async Task UpdateBookAsync(int bookId, Book book)
        {
            var url = _httpClient.BaseAddress;
            var bookToSend = _mapper.Map<Book, BookDTO>(book);
            var response = await _httpClient.PutAsJsonAsync($"{url}/Book?id={bookId}", bookToSend);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteBookAsync(int bookId)
        {
            var url = _httpClient.BaseAddress;
            var response = await _httpClient.DeleteAsync($"{url}/Book?id={bookId}");
            response.EnsureSuccessStatusCode();
        }
    }
}
