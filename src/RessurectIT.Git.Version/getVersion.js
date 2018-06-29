var VersionsExtractor = require('npm-git-version').VersionsExtractor;

module.exports = function (callback, options) 
{
    var extractor = new VersionsExtractor(options);

    extractor.process()
        .then(extractor =>
        {
            callback(/* error */ null, extractor.version);
        })
        .catch(reason =>
        {
            callback(reason, null);
        }); 
};