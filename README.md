# Poem Generator API

This repository contains our Poem Generator API. It can generate poems with the requested
number of sentences. But in reality it just checks if the [PoetryDB](https://poetrydb.org)
contains a Poem that matches and if not, fakes a Poem.

As need we scale up for our enterprise customers and want to distribute the poem
generation around the word, we are running into issues.

Please help!

## Architecture

The Poem Generator is split into separate layers:

* Presentation layer: `PoemController`
* Application layer: `PoemService`
* Infrastructure layer: `PoemDbContext` and `PoemRepository`

A SQLite database is used to store the generated poems.

## Coding Challenge

Refactor the simple Poem Generator API into a cloud-native architecture.

See the information we send you for more details.