CREATE TABLE [ClickTimeSess].[ClickWithTime] (
    [Id]          INT      IDENTITY (1, 1) NOT NULL,
    [ClickNumber] INT      DEFAULT ((0)) NOT NULL,
    [ClickTime]   TIME (7) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

