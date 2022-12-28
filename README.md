# Welcome to Unrecognized Statistical Office!

![alt text](https://github.com/skowront/SymulatorNSP2021/blob/master/mainAppView.jpg)

## Background

In 2021 in Poland Central Statistical Office (GUS) performed a National Census (NSP). However they did not publish any results about ethnic and national structure for more than a year after the census was closed. 

It is worth noting that some information was published by GUS in Q4 2022 and it was used to create a default configuration for *SymulatorNSP* application.

## SymulatorNSP

*SymulatorNSP* is an application that makes it possible for you to check how many times faster your personal PS is than Central Statistical Office. Current version is made for computers with Windows. 

### Manual for SymulatorNSP.Startup

1. Run the SymulatorNSP.Startup.exe

2. Enter number of entries that you want to create (for 38 mln it takes about 4GB RAM) and click Generate population. 

3. Query by One of available properties.
   
   1. To run a query simply type queried value like "x".
   
   2. Click Execute Query below the value.
   
   3. Observer runtime statistics and compare your results counted in seconds with more than a year that it took for GUS to calulate those on some powerfull servers. 

Additionally you can add tolerance to errors in the generated entries. For that please use text Distance Calculation methods. 

No method selected ('-') is the method that allows only exactly matching strings to be calculated, but you can also use Levenstein (implementation from [GitHub - DanHarltey/Fastenshtein: The fastest .Net Levenshtein around](https://github.com/DanHarltey/Fastenshtein)) or Hamming to automatically put some nationalities/ethnicities to buckets defined in genration configuration section.

## Generation configuration

Inside the app you can edit the config. For most of the part it has self explanatory names. So mostly you just specify available values and their probability (sum of probabilities should be 1 for property options, for example if allow 2 options John1 with p=0.6 then John2 must have p=0.4). You do it for Names, Surnames, Addresses, Religions, Nationalities, Ethnicities and Jobs. 


*Q: Why do it for some useless properties that we can not query?
A: Maybe some day the app will be extended, but the most important is that we want to simulate GUS database, which is quite huge. Of course this app does not represent the full database, but based on experience affected parts of original database could look and consume resources exactly like this simple environment.*

Keep in mind that there are 2 pairs of error-simulating properties.

- *EthnicityErrorRate* - fraction of entries that will be affected by on purpose errors for ethnicity property 

- *EthnicityErrorRateMaxImpact* - we will choose random indexes from the ethnicity property entry string and we will choose random letters to substitute the original ones. This process will be done exactly *EthnicityErrorRateMaxImpact* times for 1 entry that was earlier selected by *EthnicityErrorRate*.

- *NationalityErrorRate* - fraction of entries that will be affected by on purpose errors for nationality property

- *NationalityErrorRateMaxImpact* - we will choose random indexes from the nationality property entry string and we will choose random letters to substitute the original ones. This process will be done exactly *NationalityErrorRateMaxImpact* times for 1 entry that was earlier selected by *NationalityErrorRate*.


