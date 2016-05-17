var sqlAzure = require('msnodesqlv8');
var sql = require('mssql');
module.exports = function(router)
{

    router.get('/', function (req, res) {
        res.status(200).send("WORKING");
    });

    router.post('/api/Turn', function (req, res)
    {
        var Job =  "'" + req.body.Job + "'";
        var Action =  "'" + req.body.Action + "'";
        var sqlQuery = 'INSERT INTO Turns ([SurroundingAllyCount],[SurroundingOppositionCount],[TotalAllyCount],[TotalOppositionCount],[Job],[HealthPercent],[ManaPercent],[Move],[Action]) VALUES (' +
            req.body.SurroundingAllyCount + ', ' +
            req.body.SurroundingOppositionCount + ', ' +
            req.body.TotalAllyCount + ', ' +
            req.body.TotalOppositionCount + ', ' +
            Job + ', ' +
            req.body.HealthPercent + ', ' +
            req.body.ManaPercent + ', ' +
            req.body.Move + ', ' +
            Action + ')';

        //Azure
        var connectionString = 'Driver={SQL Server Native Client 11.0};Server=tcp:prco304-server.database.windows.net,1433;Database=PRCO304DB;Uid=danbendell@prco304-server;Pwd=HVB37mdh;Encrypt=yes;TrustServerCertificate=no;Connection Timeout=30;';

        //Live Azure
        sqlAzure.open(connectionString, function(err, conn) {
            if(err) {
                console.log(err);
            }
            console.log("HERE");
            conn.query(sqlQuery, function(err, record) {
                if(err) {
                    res.status(0).send("SELECT * FROM User - Failed");
                    console.log(err);
                }
                console.log(record);
                res.status(200).send(record);
            });
        });

        //Local
        //var config = {
        //    user: 'dan',
        //    password: 'password',
        //    server: 'localhost', // You can use 'localhost\\instance' to connect to named instance
        //    database: 'PRCO304-FinalAdventure',
        //    port: 1433,
        //
        //    options: {
        //        encrypt: true // Use this if you're on Windows Azure
        //    }
        //};
        //console.log(sqlQuery);
        //sql.connect(config).then(function() {
        //
        //    var connection = new sql.Connection(config, function(err) {
        //        if(err) {
        //            console.log("ERROR: " + err);
        //        }
        //        // Query
        //        var request = new sql.Request(connection);
        //        request.query(sqlQuery, function(err) {
        //            //... error checks
        //            if(err) {
        //                console.log(err);
        //            } else{
        //                res.status(200).send();
        //            }
        //        });
        //    });
        //}).catch(function(err) {
        //    if(err) {
        //        console.log("CONNECTION ERROR: " + err);
        //    }
        //});
    });

    router.get('/api/Turn', function (req, res)
    {
        console.log(req.headers);
        var Job =  "'" + req.headers.job + "'";

        var sqlQuery =  'SELECT [SurroundingAllyCount] ' +
                        ',[SurroundingOppositionCount] ' +
                        ',[TotalAllyCount] ' +
                        ',[TotalOppositionCount] ' +
                        ',[Job] ' +
                        ',[HealthPercent] ' +
                        ',[ManaPercent] ' +
                        ',[Move] ' +
                        ',[Action] ' +
        'FROM [PRCO304-FinalAdventure].[dbo].[Turns] ' +
        'WHERE [SurroundingAllyCount] = ' + req.headers.surroundingallycount +
        ' AND [SurroundingOppositionCount] = ' + req.headers.surroundingoppositioncount +
        ' AND [TotalAllyCount] = ' + req.headers.totalallycount +
        ' AND [TotalOppositionCount] = ' + req.headers.totaloppositioncount +
        ' AND [Job] = ' + Job +
        ' AND [HealthPercent] = ' + req.headers.healthpercent +
        ' AND [ManaPercent] = ' +  req.headers.manapercent;

        var config = {
            user: 'dan',
            password: 'password',
            server: 'localhost', // You can use 'localhost\\instance' to connect to named instance
            database: 'PRCO304-FinalAdventure',
            port: 1433,

            options: {
                encrypt: true // Use this if you're on Windows Azure
            }
        };

        //var config = {
        //    user: 'danbendell',
        //    password: 'HVB37mdh',
        //    server: 'prco304-server.database.windows.net', // You can use 'localhost\\instance' to connect to named instance
        //    database: 'PRCO304DB',
        //
        //    options: {
        //        encrypt: true // Use this if you're on Windows Azure
        //    }
        //};
        var connectionString = 'Driver={SQL Server Native Client 11.0};Server=tcp:prco304-server.database.windows.net,1433;Database=PRCO304DB;Uid=danbendell@prco304-server;Pwd=HVB37mdh;Encrypt=yes;TrustServerCertificate=no;Connection Timeout=30;';

        //Live Azure
        sqlAzure.open(connectionString, function(err, conn) {
           if(err) {
               console.log(err);
           }
            console.log("HERE");
            conn.query("SELECT * FROM Turns", function(err, record) {
                if(err) {
                    res.status(0).send("SELECT * FROM User - Failed");
                    console.log(err);
                }
                console.log(record);
                res.status(200).send(record);
            });
        });

        //Local DB
        //sql.connect(config).then(function() {
        //
        //    var connection = new sql.Connection(config, function(err) {
        //        if(err) {
        //            console.log("ERROR: " + err);
        //        }
        //        // Query
        //        var request = new sql.Request(connection);
        //        console.log(sqlQuery);
        //        request.query(sqlQuery, function(err, recordset) {
        //            //... error checks
        //            if(err) {
        //                console.log(err);
        //            }
        //            //console.dir(recordset);
        //            res.status(200).send(recordset);
        //
        //        });
        //    });
        //}).catch(function(err) {
        //    if(err) {
        //        console.log("CONNECTION ERROR: " + err);
        //    }
        //});


    });


};