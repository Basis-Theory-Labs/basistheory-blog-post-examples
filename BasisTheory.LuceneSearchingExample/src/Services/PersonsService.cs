using System.Linq.Expressions;
using System.Text;
using BasisTheory.LuceneSearchingExample.Extensions.Expressions;
using BasisTheory.LuceneSearchingExample.entities;
using BasisTheory.LuceneSearchingExample.Models.Requests;
using Lucene.Net.Analysis.Core;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.QueryParsers.Classic;
using Lucene.Net.Search;
using Lucene.Net.Util;
using Microsoft.EntityFrameworkCore;

namespace BasisTheory.LuceneSearchingExample.Services;

public interface IPersonsService
{
    public IQueryable<Person> SearchPersons(SearchPersonsRequest request);
}

public class PersonsService : IPersonsService
{
    private readonly SocietyDbContext _societyDbContext;

    public PersonsService(SocietyDbContext societyDbContext)
    {
        _societyDbContext = societyDbContext;
    }

    public IQueryable<Person> SearchPersons(SearchPersonsRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Query)) return _societyDbContext.Persons.AsNoTracking();

        using var analyzer = new WhitespaceAnalyzer(LuceneVersion.LUCENE_48);

        var parser = new QueryParser(LuceneVersion.LUCENE_48, null, analyzer);

        var searchFilter = GetTerms(parser.Parse(request.Query));

        return _societyDbContext.Persons.AsNoTracking().Where(searchFilter);
    }

    private Expression<Func<Person, bool>> GetTerms(Query query) =>
        query switch
        {
            TermQuery termQuery => CreateTermExpression(termQuery),
            TermRangeQuery => throw new InvalidOperationException(),
            PhraseQuery phraseQuery => ParsePhraseQuery(phraseQuery),
            BooleanQuery booleanQuery => ParseBooleanQuery(booleanQuery),
            _ => throw new InvalidOperationException()
        };

    private Expression<Func<Person, bool>> CreateTermExpression(TermQuery termQuery)
    {
        var searchValue = termQuery.Term.Text;

        return termQuery.Term.Field switch
        {
            "firstName" => person => person.FirstName == searchValue,
            "lastName" => person => person.LastName == searchValue,
            "ssn" => person => person.Ssn == searchValue,
            "favoriteFood" => person => person.FavoriteFood == searchValue,
            _ => throw new InvalidOperationException()
        };
    }

    private Expression<Func<Person, bool>> ParsePhraseQuery(PhraseQuery phraseQuery) =>
        phraseQuery.GetTerms()[0].Field switch
        {
            "favoriteFood" => person => person.FavoriteFood == BuildPhraseQueryValue(phraseQuery),
            _ => throw new InvalidOperationException()
        };

    private Expression<Func<Person, bool>> ParseBooleanQuery(BooleanQuery query)
    {
        var startExpression = GetTerms(query.Clauses[0].Query);

        return query.Clauses.Skip(1).Fold(
            startExpression,
            (filter, clause) =>
            {
                var clauseExpression = GetTerms(clause.Query);
                return ConcatBooleanExpressions(clause, filter, clauseExpression);
            });
    }

    private Expression<Func<Person, bool>> ConcatBooleanExpressions(
        BooleanClause clause,
        Expression<Func<Person, bool>> first,
        Expression<Func<Person, bool>> second) =>
        clause.Occur switch
        {
            Occur.MUST => first.And(second),
            Occur.SHOULD => first.Or(second),
            _ => throw new InvalidOperationException()
        };

    private string BuildPhraseQueryValue(PhraseQuery phraseQuery)
    {
        var stringBuilder = new StringBuilder();
        foreach (var term in phraseQuery.GetTerms())
        {
            stringBuilder.Append($"{term.Text} ");
        }

        return stringBuilder.ToString().Trim();
    }
}