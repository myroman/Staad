Use Staad

IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'tbWord')) BEGIN
 drop table tbWord
END

IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'tbWordlist')) BEGIN
 drop table tbWordlist
END

-- ********* Create tables *************

go
create table tbWordlist
(
[id] int identity(1, 1),
[name] varchar(200) not null,
[created] datetime not null,

constraint PK_WordlistId primary key([id])
)

go
create table tbWord
(
    [id] int identity(1, 1),
    [wordlistId] int not null,
    [original] nvarchar(150) not null,
    [definition] nvarchar(700) not null,
    [example] nvarchar(MAX) null,
    
    constraint PK_WordID primary key([id]),
    constraint FK_Word_WordlistID foreign key(wordlistId) references tbWordlist(id)
)

insert into tbWordlist(name, created) values('Feelings', getDate())
insert into tbWordlist(name, created) values('Food', getDate())
insert into tbWordlist(name, created) values('Clothes', getDate())

insert into tbWord([wordlistId], [original], [definition], [example])
values(1, 'callous', 'showing or having an insensitive and cruel disregard for others',
'his callous comments about the murder made me shiver')
insert into tbWord([wordlistId], [original], [definition], [example])
values(1, 'sensitive', 'having or displaying a quick and delicate appreciation of others'' feelings', 'I pay tribute to the Minister for his sensitive handling of the bill')
insert into tbWord([wordlistId], [original], [definition], [example])
values(1, 'genial', 'friendly and cheerful', 'my genial friend')
insert into tbWord([wordlistId], [original], [definition], [example])
values(1, 'treacherous', 'guilty of or involving betrayal or deception', 'a treacherous Gestapo agent')
insert into tbWord([wordlistId], [original], [definition])
values(1, 'vivacious', '(especially of a woman) attractively lively and animated')
insert into tbWord([wordlistId], [original], [definition], [example])
values(1, 'animated', 'full of life or excitement; lively', '')
insert into tbWord([wordlistId], [original], [definition], [example])
values(1, 'reluctant', 'unwilling and hesitant', '')
insert into tbWord([wordlistId], [original], [definition], [example])
values(1, 'unfair', 'not based on or behaving according to the principles of equality and justice', 'unfair rules of the game')
insert into tbWord([wordlistId], [original], [definition], [example])
values(1, 'reticent', 'not revealing one’s thoughts or feelings readily', 'she was reticent about her past')
insert into tbWord([wordlistId], [original], [definition])
values(1, 'buoyant', 'cheerful and optimistic')
insert into tbWord([wordlistId], [original], [definition], [example])
values(1, 'distressed', 'suffering from extreme anxiety, sorrow, or pain', 'I was distressed at the news of his death')
insert into tbWord([wordlistId], [original], [definition], [example])
values(1, 'meek', 'very quiet and always doing what other people want', 'he was not like a man-meek creature...')
insert into tbWord([wordlistId], [original], [definition], [example])
values(1, 'flatterer', 'who says nice thing about someone or pretend that he admire them but do not really mean it', '')
insert into tbWord([wordlistId], [original], [definition], [example])
values(1, 'smarmy', 'who behaves to gain approval and flatters as if he wants to get what he want in a way that is regarded as insincere', 'she is a real smarmer')
insert into tbWord([wordlistId], [original], [definition], [example])
values(1, 'toady', 'who behaves obsequiously to someone important', '')
insert into tbWord([wordlistId], [original], [definition], [example])
values(1, 'naughty', 'a ~ child behaves badly', '')



insert into tbWord([wordlistId], [original], [definition])
values(2, 'sesame', 'a tall annual herbaceous plant of tropical and subtropical areas of the Old World, cultivated for its oil-rich seeds')