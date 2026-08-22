using DominoPontaDeQuina.Repository.Context;
using Microsoft.EntityFrameworkCore;

using var db = new DominoDbContext();
await db.Database.MigrateAsync();
Console.WriteLine("Migration aplicada com sucesso. Banco: domino.db");
