using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAR.Config.Stores
{
    //private class OLDSvrSettings
    //{

        //' ***********************  Version 2014-06-20   ************************'

        //'ACER Aspire Laptop
        //'Friend Const StrSQLCon As String = "data Source=(local);Initial Catalog=DATA000;Persist Security Info=True;User ID=sa;Password=sp0rpk0qt0x"

        //'TEST Virtual Machine W2K3 + SQL2K
        //'Friend Const StrSQLCon As String = "data Source=(local)\SQL2K;Initial Catalog=DATA000;Persist Security Info=True;User ID=sa;Password=sp0rpk0qt0x"

        //'Consorcio Rosario Ortiz
        //'Friend Const StrSQLCon As String = "data source=MAR-SERVER;initial catalog=DATA000;persist security info=True;Min Pool Size=10;user id=sa2;pwd=p0l9i8j7"
        //'Private AllowedHosts As String() = {"wrosario.dynip.com", "wrosario.dynip.com:7500", "wrosario.ddns.net", "wrosario.ddns.net:80"}

        //'JOSELITO SPORT
        //'Friend Const StrSQLCon As String = "data source=CENTRAL;initial catalog=DATA003;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt3z"
        //'Private AllowedHosts As String() = {"bj.dynip.com", "bj.dynip.com:80", "joselito.ddns.net", "joselito.ddns.net:80"}

        //'CONSORCIO MENDOZA 
        //'Friend Const StrSQLCon As String = "data source=MENDOZA-SERVER;initial catalog=data004;persist security info=True;Min Pool Size=10;user id=sa2;pwd=mar004.seguridad2.brutal2"

        //'MIGUEL SPORT FAMOSA / 172.22.7.41 / 200.42.222.192  pw: admin20042222192 / lafamosa.dynip.com
        //'Friend Const StrSQLCon As String = "data source=MIGUEL-SPORT;initial catalog=DATA005;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt5z"
        //'Private AllowedHosts As String() = {"lafamosa.dynip.com", "lafamosa.dynip.com:80", "lafamosa.ddns.net", "lafamosa.ddns.net:80"}

        //'ALBERTO SPORT
        //'Friend Const StrSQLCon As String = "data source=MAR-SERVIDOR;initial catalog=DATA006;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt6z"

        //'RICHARD ORTIZ / cbo.ddns.net
        //'Friend Const StrSQLCon As String = "data source=ORTIZ-MAR\SQL2KMAR;initial catalog=DATA007;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt7z"
        //'Friend Const StrSQLCon As String = "data source=MAIN01;initial catalog=DATA007;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt0z"
        //'Private AllowedHosts As String() = {"cbo.ddns.net", "cbo.ddns.net:80"}

        //'HECTOR SPORT / hs2007.dynip.com
        //'Friend Const StrSQLCon As String = "data source=MAR-HECTOR;initial catalog=DATA008;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt8z"
        //'Private AllowedHosts As String() = {"bancahs.ddns.net", "bancahs.ddns.net:80", "hs2007.dynip.com", "hs2007.dynip.com:80", "hs2007.dynip.com:7500"}

        //'FGOMEZ SPORT - Version 2014-06-20
        //'Friend Const StrSQLCon As String = "data source=MAR-FGOMEZ;initial catalog=DATA009;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt9z"
        //'Private AllowedHosts As String() = {"fgomez.dynip.com", "fgomez.dynip.com:80", "fgomez.ddns.net", "fgomez.ddns.net:80"}

        //'BANCAS GUSTAVO - Jose Garcia - Version 2014-05-01
        //'Friend Const StrSQLCon As String = "data Source=STARLING-MAR;initial catalog=DATA010;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st1rpk3qt4z"
        //'Friend Const StrSQLCon As String = "data source=bgserver2;initial catalog=DATA010;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk1qt0z"
        //'Friend Const StrSQLCon As String = "data source=MAIN01;initial catalog=DATA010;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt0z"
        //'Private AllowedHosts As String() = {"bancagustavo.ddns.net", "bancagustavo.ddns.net:80", "gustavo.dynip.com", "gustavo.dynip.com:80"}

        //'BANCAS CHICHI
        //'Friend Const StrSQLCon As String = "data source=CENTRALCHICHI;initial catalog=DATA011;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk1qt1z"

        //'EVARISTO SPORT - ces.dynip.com - Version 2014-04-01
        //'Friend Const StrSQLCon As String = "data source=MARCENTRAL;initial catalog=DATA012;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk1qt2z"
        //'Private AllowedHosts As String() = {"ces.dynip.com", "ces.dynip.com:80"}

        //'DANIEL SAN CRISTOBAL - bancadaniel.ddns.net
        //'Friend Const StrSQLCon As String = "data source=DA-CENTRAL;initial catalog=DATA013;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk1qt3z"
        //'Friend Const StrSQLCON As String = "data source=MAIN01;initial catalog=DATA013;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt0z"

        //'CENTRAL MARIA RODRIGUEZ 172.16.6.199
        //'Friend Const StrSQLCon As String = "data source=MRARGUEZ;initial catalog=MAR014;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk1qt4z"

        //'CENTRAL RUBEN-HNA
        //'Friend Const StrSQLCon As String = "data source=RUBEN-HNA;initial catalog=DATA015;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk1qt5z"

        //'CENTRAL MARTIN-MAR / 200.42.225.216 / La Fuerte 1
        //'Friend Const StrSQLCon As String = "data source=MARTIN-MAR;initial catalog=DATA016;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk1qt6z"

        //'CENTRAL ROSANNA CASTILLO / 172.16.6.211
        //'Friend Const StrSQLCon As String = "data source=MARCENTRAL;initial catalog=DATA017;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk1qt7z"
        //'Friend Const StrSQLCon As String = "data source=MAIN01;initial catalog=DATA017;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt0z"
        //'Private AllowedHosts As String() = {"rosannar.dynip.com", "rosannar.dynip.com:80"}

        //'CENTRAL CIBAO / 172.16.31.2 -> 6
        //'Friend Const StrSQLCon As String = "data source=MAR-ROBERTO;initial catalog=data018;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk1qt8z"
        //'Private AllowedHosts As String() = {"cibao.dynip.com", "cibao.dynip.com:80"}

        //'CENTRAL AMBIORIX - Famosa3 - ambiorix2006.dynip.com
        //'Friend Const StrSQLCon As String = "data source=MAR-AMBIORX;initial catalog=DATA019;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk1qt9z"
        //'Private AllowedHosts As String() = {"ambiorix2006.dynip.com", "ambiorix2006.dynip.com:80", "ambiorix2006.dynip.com:7500"}

        //'CENTRAL DEMO 1A / 172.16.14.240
        //'Friend Const StrSQLCon As String = "data source=marserver;initial catalog=MAR020;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk2qt0z"

        //'CENTRAL TAVERAS / 172.100.1.1 / 172.150.27.10 / tavllldomtit
        //'Friend Const StrSQLCon As String = "data source=TAVERAS-SVR2;initial catalog=DATA021;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk2qt1z"

        //'CENTRAL CHELIN / 172.40.2.10 /  chelitodomtito
        //'Friend Const StrSqlCon = "data source=CHELIN-MAR;initial catalog=MAR022;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk2qt2z"

        //'CENTRAL WILLIAM-FELIX / 172.16.39.2 / 192.65.30.1 / wfi621LLe
        //'Friend Const StrSqlCon = "data source=WF-MAR;initial catalog=MAR023;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk2qt3z"

        //'EL TESORO
        //'Friend Const StrSQLCON As String = "data source=TESORO-MAR;initial catalog=DATA024;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk2qt4z"
        //'Friend Const StrSQLCon As String = "data source=MAIN01;initial catalog=DATA024;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt0z"
        //'Private AllowedHosts As String() = {"eltesoro2006.dynip.com", "eltesoro2006.dynip.com:7500"}

        //'ARLEQUIN / irq4irq3king / 192.168.75.5
        //'Friend Const StrSQLCON As String = "data source=MAR-ARLEQ;initial catalog=DATA025;persist security info=True;Min Pool Size=10;user id=sa2;pwd=sp0rpk2qt5z"

        //'SANDRO / abc975poli / 172.16.40.2   / sandro2007.dynip.com
        //'Friend Const StrSQLCON As String = "data source=MAR-X;initial catalog=DATA026;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk2qt6z"
        //'Friend Const StrSQLCON As String = "data source=MAIN01;initial catalog=DATA026;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt0z"

        //'LA CONFIANZA - JOSELITO / 123LONDON / confianza2006.dynip.com
        //'Friend Const StrSQLCON As String = "data source=MAR-conf;initial catalog=DATA027;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk2qt7z"
        //'Private AllowedHosts As String() = {"confianza2006.dynip.com", "confianza2006.dynip.com:80"}

        //'SPC - Miami / 1q2w3e / 66.236.249.115
        //'Friend Const StrSQLCON As String = "data source=SPC-LOTO;initial catalog=DATA028;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk2qt8z"

        //'CONSORCIO LEXUS MATECOMSA - Version 2014-04-01
        //'Friend Const StrSQLCon As String = "data source=DINERO-MAR;initial catalog=DATA029;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk2qt7z"
        //'Friend Const StrSQLCON As String = "data source=MAIN01;initial catalog=DATA029;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt0z"
        //'Private AllowedHosts As String() = {"bancalexus.ddns.net", "bancalexus.ddns.net:80", "bancalexus.ddns.net:8888", "peluche.dynip.com", "peluche.dynip.com:80", "peluche.dynip.com:8888"}

        //'CONSORCIO PEGUERO  192.168.60.10
        //'Friend Const StrSQLCon As String = "data source=MAR-PEGUERO;initial catalog=DATA030;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk3qt0z"

        //'CONSORCIO FRANKLYN - ROMANA1 - 192.168.0.10
        //'Friend Const StrSQLCon As String = "data source=MAR-FRANKLIN;initial catalog=DATA031;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk3qt1z"

        //'CONSORCIO AGUERO - ROMANA2 - 66.98.69.12
        //'Friend Const StrSQLCon As String = "data source=MAR-AGUERO;initial catalog=DATA032;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk3qt2z"

        //'CONSORCIO CANASTO - 172.16.16.10
        //'Friend Const StrSQLCon As String = "data source=MARCANASTO;initial catalog=DATA033;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk2qt7z"

        //'CONSORCIO MARY - bmary.dynip.com
        //'Friend Const StrSQLCon As String = "data source=MAR-BMARY;initial catalog=DATA034;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk3qt4z"

        //'CONSORCIO El Regreso - regreso.dynip.com
        //'Friend Const StrSQLCon As String = "data source=MAR-BCARGUEZ;initial catalog=DATA035;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk3qt5z"

        //'CONSORCIO RODRIGUEZ - erodriguez.dynip.com
        //'Friend Const StrSQLCon As String = "data source=MAR-BCARGUEZ;initial catalog=DATA035;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk3qt5z"

        //'MIGUEL LA FAMOSA 2 - 172.40.59.10
        //'Friend Const StrSQLCon As String = "data source=CENTRAL-DARIO;initial catalog=DATA065;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk6qt5z"

        //'LA FUERTE 2 (Rafaela) - yordi2.ddns.net
        //'Friend Const StrSQLCon As String = "data source=MARTIN2-MAR;initial catalog=DATA037;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk3qt7z"
        //'Private AllowedHosts As String() = {"yordi2.ddns.net", "yordi2.ddns.net:80"}

        //'PICA PICA - pica.dynip.com
        //'Friend Const StrSQLCon As String = "data source=PICA-MAR;initial catalog=DATA038;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk3qt8z"
        //'Friend Const StrSQLCon As String = "data source=MAIN01;initial catalog=DATA038;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt0z"
        //'Private AllowedHosts As String() = {"pica.dynip.com", "pica.dynip.com:80"}

        //'CONSORCIO QUISQUEYANO (antiguo Elite Madrid) - elitemadrid.dynip.com - elitemadrid2.dynip.com - quisqueyano.ddns.net - Version 2014-06-20
        //'Friend Const StrSQLCon As String = "data source=MAR-ELITE;initial catalog=DATA039;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk3qt9z"
        //'Private AllowedHosts As String() = {"quisqueyano.ddns.net", "quisqueyano.ddns.net:80"}

        //'CONSORCIO Octavio - octavio.dynip.com
        //'Friend Const StrSQLCon As String = "data source=OCTAVIO-SVR;initial catalog=DATA040;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk4qt0z"

        //'CONSORCIO DURAN - duran.dynip.com
        //'Friend Const StrSQLCon As String = "data source=DURAN-SVR;initial catalog=DATA041;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk4qt1z"

        //'CONSORCIO DANNY - danny.dynip.com
        //'Friend Const StrSQLCon As String = "data source=DANNY-SVR;initial catalog=DATA042;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk4qt2z"

        //'CONSORCIO DELCIO - delcio.dynip.com
        //'Friend Const StrSQLCon As String = "data source=DELCIO-SVR;initial catalog=DATA043;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk4qt3z"
        //'Friend Const StrSQLCon As String = "data source=MAIN01;initial catalog=DATA043;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt0z"
        //'Private AllowedHosts As String() = {"delcio.dynip.com", "delcio.dynip.com:80"}

        //'CONSORCIO EL MANGO - mango.dynip.com
        //'Friend Const StrSQLCon As String = "data source=MANGO-SVR;initial catalog=DATA044;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk4qt4z"

        //'CONSORCIO DOMITEL ESPANA - domitel.dynip.com
        //'Friend Const StrSQLCon As String = "data source=DOMITEL-SVR;initial catalog=DATA045;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk4qt5z"

        //'CONSORCIO SANDY - sandy.dynip.com
        //'Friend Const StrSQLCon As String = "data source=SANDY-SVR;initial catalog=DATA046;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk4qt6z"

        //'CONSORCIO CRISTIAN - cristian.dynip.com
        //'Friend Const StrSQLCon As String = "data source=CRISTIAN-SVR;initial catalog=DATA047;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk4qt7z"

        //'CONSORCIO Ramon Emilio España - re.dynip.com
        //'Friend Const StrSQLCon As String = "data source=RE-MAR\SQL;initial catalog=DATA048;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk4qt8z"

        //'CONSORCIO ODR - odr.dynip.com
        //'Friend Const StrSQLCon As String = "data source=MAR-ODR;initial catalog=DATA049;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk4qt9z"

        //'CONSORCIO BENNY - benny.dynip.com
        //'Friend Const StrSQLCon As String = "data source=BENNY-MAR;initial catalog=DATA050;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk5qt0z"

        //'CONSORCIO HOVENSA - hovensa.dynip.com
        //'Friend Const StrSQLCon As String = "data source=HOV-MAR;initial catalog=DATA051;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk5qt1z"

        //'CONSORCIO EL SUEÑO - elsueno.dynip.com
        //'Friend Const StrSQLCon As String = "data source=ELSUENO-MAR;initial catalog=DATA052;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk5qt2z"

        //'CONSORCIO SUERTE ESPANA - mbsuerte.dynip.com
        //'Friend Const StrSQLCon As String = "data source=MBSUERTE-MAR;initial catalog=DATA053;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk5qt3z"

        //'CONSORCIO PRESIDENTE SPORT - pte.dynip.com
        //'Friend Const StrSQLCon As String = "data source=PTE-MAR;initial catalog=DATA054;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk5qt4z"

        //'CONSORCIO ATLANTIC CITY - atlcity.dynip.com
        //'Friend Const StrSQLCon As String = "data source=ESTRELLA-MAR;initial catalog=DATA055;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk5qt5z"
        //'Friend Const StrSQLCon As String = "data source=MAIN01;initial catalog=DATA055;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt0z"
        //'Private AllowedHosts As String() = {"atlcity.dynip.com", "atlcity.dynip.com:80"}

        //'CONSORCIO CHEPE - chepe.dynip.com
        //'Friend Const StrSQLCon As String = "data source=CHEPE-MAR;initial catalog=DATA056;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk5qt6z"

        //'CONSORCIO MONEYMAN - moneyman.dynip.com
        //'Friend Const StrSQLCon As String = "data source=MONEYMAN-MAR;initial catalog=DATA057;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk5qt7z"

        //'CONSORCIO NELSON-NR - nr.dynip.com - Version 2014-04-01
        //'Friend Const StrSQLCon As String = "data source=MAR-NELSON;initial catalog=DATA058;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk5qt8z"

        //'CONSORCIO MELO - melo.dynip.com
        //'Friend Const StrSQLCon As String = "data source=BANCA-MELO;initial catalog=DATA059;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk5qt9z"
        //'Friend Const StrSQLCON As String = "data source=MAIN01;initial catalog=DATA059;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt0z"

        //'CONSORCIO ESTRELLA - estrella.dynip.com
        //'Friend Const StrSQLCon As String = "data source=ESTRELLA-SVR;initial catalog=DATA060;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk6qt0z"

        //'CONSORCIO ESTRELLA - estrella.dynip.com
        //'Friend Const StrSQLCon As String = "data source=ESTRELLA-SVR;initial catalog=DATA060;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk6qt0z"

        //'CONSORCIO OMAR - omar.dynip.com
        //'Friend Const StrSQLCon As String = "data source=OMAR-SVR;initial catalog=DATA061;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk6qt1z"

        //'CONSORCIO NICOLAS - nicolas.dynip.com
        //'Friend Const StrSQLCon As String = "data source=NICOLAS-SVR;initial catalog=DATA062;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk6qt2z"

        //'BANCA FEBRILLET - bfe.dynip.com
        //'Friend Const StrSQLCon As String = "data source=FEBRILLET;initial catalog=DATA063;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk6qt3z"

        //'CONSORCIO DANIEL - daniel.dynip.com
        //'Friend Const StrSQLCon As String = "data source=DANIEL-MAR;initial catalog=DATA064;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk6qt4z"

        //'CONSORCIO MOSQUEA - mosquea.dynip.com
        //'Friend Const StrSQLCon As String = "data source=CENTRAL-DARIO;initial catalog=DATA065;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk6qt5z"

        //'CONSORCIO IMPERIO - imperio.dynip.com
        //'Friend Const StrSQLCon As String = "data source=IMPERIO;initial catalog=DATA066;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk6qt6z"

        //'CONSORCIO JULIOCESAR - jc.dynip.com
        //'Friend Const StrSQLCon As String = "data source=JCESARMAR;initial catalog=DATA067;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk6qt7z"
        //'Friend Const StrSQLCON As String = "data source=MAIN01;initial catalog=DATA067;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt0z"

        //'CONSORCIO DURAN GARCIA - dg.dynip.com
        //'Friend Const StrSQLCon As String = "data source=DURANGARCIAMAR;initial catalog=DATA068;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk6qt8z"

        //'CONSORCIO LUNA NUEVA - lnueva.dynip.com
        //'Friend Const StrSQLCon As String = "data source=LUNANUEVAMAR;initial catalog=DATA069;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk6qt9z"

        //'MATECOMSA 2
        //'Friend Const StrSQLCon As String = "data source=DINERO-MAR;initial catalog=DATA070;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk2qt7z"

        //'BANCAS JDC - jdc.dynip.com
        //'Friend Const StrSQLCon As String = "data source=MAR-JDC;initial catalog=DATA071;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk7qt1z"

        //'BANCAS LOS COMPADRES - compadres.dynip.com
        //'Friend Const StrSQLCon As String = "data source=MARCOMPADRES;initial catalog=DATA072;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk7qt2z"

        //'FRANCISCO PR - fco.dynip.com (San Miguel)
        //'Friend Const StrSQLCon As String = "data source=FRANCISCO-MAR;initial catalog=DATA073;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk7qt3z"

        //'RODRIGUEZ CONFONDO - ESPANA - confondo.dynip.com
        //'Friend Const StrSQLCon As String = "data source=CONFONDO-MAR;initial catalog=DATA074;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk7qt4z"

        //'NELLY - nelly.dynip.com
        //'Friend Const StrSQLCon As String = "data source=NELLY-MAR;initial catalog=DATA075;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk7qt5z"

        //'EL BILLETE - billete.dynip.com
        //'Friend Const StrSQLCon As String = "data source=BILLETE-MAR;initial catalog=DATA076;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk7qt6z"

        //'HIDALGO - hidalgo.dynip.com
        //'Friend Const StrSQLCon As String = "data source=MAR-HIDALGO;initial catalog=DATA077;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk7qt7z"

        //'JuanCaribe - juanc.dynip.com
        //'Friend Const StrSQLCon As String = "data source=MAR-JUANC;initial catalog=DATA078;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk7qt8z"
        //'Friend Const StrSQLCON As String = "data source=MAIN01;initial catalog=DATA078;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt0z"

        //'MOREL - morel.dynip.com
        //'Friend Const StrSQLCon As String = "data source=MAR-MOREL;initial catalog=DATA079;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk7qt9z"

        //'Consorcio Luis - Ex-Emely PR - emely.dynip.com - Version 2014-06-20
        //'Friend Const StrSQLCon As String = "data source=EMELY-MAR;initial catalog=DATA080;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk8qt0z"
        //'Private AllowedHosts As String() = {"emely.dynip.com", "emely.dynip.com:80"}

        //'Consorcio RC Puerto Rico (Anterior: HM PR) - rc.dynip.com
        //'Friend Const StrSQLCon As String = "data source=HM-MAR;initial catalog=DATA081;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk8qt1z"

        //'Consorcio M y R - mr.dynip.com
        //'Friend Const StrSQLCon As String = "data source=MAR-MR;initial catalog=DATA082;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk8qt2z"

        //'Consorcio Soriano - soriano.dynip.com
        //'Friend Const StrSQLCon As String = "data source=MAR-Soriano;initial catalog=DATA083;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk8qt3z"

        //'Consorcio Landestoy - landestoy.dynip.com
        //'Friend Const StrSQLCon As String = "data source=MAR-Land;initial catalog=DATA084;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk8qt4z"

        //'Consorcio Junior - junior.dynip.com
        //'Friend Const StrSQLCon As String = "data source=MAR-Junior;initial catalog=DATA085;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk8qt5z"

        //'Consorcio Albery - albery.dynip.com
        //'Friend Const StrSQLCon As String = "data source=MAR-Albery;initial catalog=DATA086;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk8qt6z"

        //'Consorcio Melissa - melissa.dynip.com
        //'Friend Const StrSQLCon As String = "data source=MAR-Melissa;initial catalog=DATA087;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk8qt7z"
        //'Friend Const StrSQLCON As String = "data source=MAIN01;initial catalog=DATA087;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt0z"

        //'Consorcio GN - gn.dynip.com
        //'Friend Const StrSQLCon As String = "data source=MAR-GN;initial catalog=DATA088;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk8qt8z"

        //'Consorcio SH - sh.dynip.com - Version 2014-05-01
        //'Friend Const StrSQLCon As String = "data source=MAR-SH;initial catalog=DATA089;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk8qt9z"

        //'Consorcio El Gordo - elgordo.dynip.com
        //'Friend Const StrSQLCon As String = "data source=MAR-Gordo;initial catalog=DATA090;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk9qt0z"

        //'Consorcio Pedro Guerrero Espana - pg.dynip.com
        //'Friend Const StrSQLCon As String = "data source=MAR-PG;initial catalog=DATA091;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk9qt1z"
        //'Friend Const StrSQLCon As String = "data source=MAIN01;initial catalog=DATA091;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt0z"
        //'Private AllowedHosts As String() = {"pg.dynip.com", "pg.dynip.com:80"}

        //'Consorcio Dario Tati PR - dt.dynip.com
        //'Friend Const StrSQLCon As String = "data source=MAR-DT;initial catalog=DATA092;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk9qt2z"

        //'Consorcio Yampool - yampool.dynip.com
        //'Friend Const StrSQLCon As String = "data source=MAR-YAMPOOL;initial catalog=DATA093;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk9qt3z"

        //'Consorcio J&M jm.dynip.com
        //'Friend Const StrSQLCon As String = "data source=MAR-JM;initial catalog=DATA094;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk9qt4z"

        //'Consorcio Sonrie sonrie.dynip.com
        //'Friend Const StrSQLCon As String = "data source=MAR-Sonrie;initial catalog=DATA095;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk9qt5z"

        //'MARTIN LA FUERTE 3 - lafuerte-b.dynip.com - Version 2014-05-01
        //'Friend Const StrSQLCon As String = "data source=MARTIN3-MAR;initial catalog=DATA096;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk9qt6z"
        //'Friend Const StrSQLCON As String = "data source=MAIN01;initial catalog=DATA096;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt0z"

        //'ROKAS - 192.168.200.251
        //'Friend Const StrSQLCon As String = "data source=ROKAS-MAR;initial catalog=DATA097;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk9qt7z"

        //'LA FORTUNA - lafortuna.dynip.com
        //'Friend Const StrSQLCON As String = "data source=MAIN01;initial catalog=DATA097;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt0z"

        //'SyM - sym.dynip.com
        //'Friend Const StrSQLCon As String = "data source=SYM-MAR;initial catalog=DATA098;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk9qt8z"

        //'Consorcio Caricia - caricia.dynip.com
        //'Friend Const StrSQLCon As String = "data source=CARICIA-MAR;initial catalog=DATA099;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk9qt9z"

        //'Consorcio Jordy - jordy.dynip.com / jordy.ddns.net
        //'Friend Const StrSQLCon As String = "data source=JORDY-MAR;initial catalog=DATA100;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st1rpk0qt0z"
        //'Friend Const StrSQLCON As String = "data source=MAIN01;initial catalog=DATA100;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt0z"
        //'Private AllowedHosts As String() = {"jordy.ddns.net", "jordy.ddns.net:80", "jordy.dynip.com", "jordy.dynip.com:80"}

        //'Consorcio C&G Puerto Rico - bancacg.ddns.net (antiguo cq.dynip.com)
        //'Friend Const StrSQLCon As String = "data source=CQ-MAR;initial catalog=DATA104;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st1rpk0qt4z"
        //'Friend Const StrSQLCON As String = "data source=MAIN01;initial catalog=DATA104;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt0z"
        //'Private AllowedHosts As String() = {"bancacg.ddns.net", "bancacg.ddns.net:80"}

        //'Kandi Lottery Lacroes - kandi.dynip.com
        //'Friend Const StrSQLCon As String = "data source=KANDI-MAR;initial catalog=DATA105;persist security info=True;Min Pool Size=10;user id=sa;pwd=sp1rpk0qt5x"
        //'Friend Const StrSQLCON As String = "data source=MAIN01;initial catalog=DATA105;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt0z"

        //'Bancas Ransel - ransel.dynip.com
        //'Friend Const StrSQLCon As String = "data source=RANSEL-MAR;initial catalog=DATA106;persist security info=True;Min Pool Size=10;user id=sa;pwd=sp1rpk0qt6x"

        //'Bancas Imperio2 - imperio2.dynip.com
        //'Friend Const StrSQLCon As String = "data source=IMPERIO2-MAR;initial catalog=DATA107;persist security info=True;Min Pool Size=10;user id=sa;pwd=sp1rpk0qt7x"
        //'Friend Const StrSQLCON As String = "data source=MAIN01;initial catalog=DATA107;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt0z"

        //'Bancas Nelson Cruz - nelsocruz.dynip.com
        //'Friend Const StrSQLCon As String = "data source=MAR-NELCRUZ;initial catalog=DATA108;persist security info=True;Min Pool Size=10;user id=sa;pwd=sp1rpk0qt8x"

        //'Bancas Febrillet - febrillet.dynip.com
        //'Friend Const StrSQLCon As String = "data source=MAR-FEBRILLET;initial catalog=DATA109;persist security info=True;Min Pool Size=10;user id=sa;pwd=sp1rpk0qt9x"

        //'Bancas Sin Rival - sinrival.dynip.com
        //'Friend Const StrSQLCon As String = "data source=MAR-SINRIVAL;initial catalog=DATA110;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st1rpk1qt0z"
        //'Friend Const StrSQLCON As String = "data source=MAIN01;initial catalog=DATA110;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt0z"

        //'Consorcio UNIEMPRESA - ue.dynip.com
        //'Friend Const StrSQLCon As String = "data source=UNIBANCA1;initial catalog=DATA111;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st1rpk1qt1z"

        //'Consorcio LA GANADORA - ganadora.dynip.com
        //'Friend Const StrSQLCon As String = "data source=MARGANADORA;initial catalog=DATA112;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st1rpk1qt2z"

        //'Consorcio La Brava - labrava.dynip.com
        //'Friend Const StrSQLCon As String = "data source=MAR-BRAVA;initial catalog=DATA113;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st1rpk1qt3z"

        //'Consorcio Oliver - oliver.dynip.com
        //'Friend Const StrSQLCon As String = "data source=CONSORCI-OLIVER;initial catalog=DATA114;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st1rpk1qt4z"

        //'Consorcio Alis - alis.dynip.com
        //'Friend Const StrSQLCon As String = "data source=MAR-ALIS;initial catalog=DATA115;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st1rpk1qt5z"

        //'Consorcio JyM - jm.dynip.com
        //'Friend Const StrSQLCon As String = "data source=MAR-JM;initial catalog=DATA116;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st1rpk1qt6z"

        //'Consorcio Tony Razon- tr.dynip.com
        //'Friend Const StrSQLCon As String = "data source=MAR-TONY;initial catalog=DATA117;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st1rpk1qt7z"

        //'Consorcio La Millonaria - millonaria.dynip.com
        //'Friend Const StrSQLCon As String = "data source=MAR-MILLONAR;initial catalog=DATA118;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st1rpk1qt8z"

        //'Consorcio La Esperanza - esperanza.dynip.com
        //'Friend Const StrSQLCon As String = "data source=MAR-LE;initial catalog=DATA119;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st1rpk1qt9z"

        //'Consorcio Carnelis - dc.dynip.com
        //'Friend Const StrSQLCon As String = "data source=MAR-DC;initial catalog=DATA120;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st1rpk2qt0z"

        //'Consorcio Castillo - castillo.dynip.com
        //'Friend Const StrSQLCon As String = "data source=MAR-Castillo;initial catalog=DATA121;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st1rpk2qt1z"

        //'Consorcio Malena - malena.dynip.com
        //'Friend Const StrSQLCon As String = "data source=MalenaCpa;initial catalog=DATA122;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st1rpk2qt2z"

        //'Consorcio CARIBBEAN - caribbean.dynip.com
        //'Friend Const StrSQLCon As String = "data source=MAR-CARIBBEAN;initial catalog=DATA123;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st1rpk2qt3z"

        //'Consorcio DOLMAV - dolmav.dynip.com
        //'Friend Const StrSQLCon As String = "data source=MAR-DOLMAV;initial catalog=DATA124;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st1rpk2qt4z"

        //'Consorcio R&G Puerto Rico - rg.dynip.com
        //'Friend Const StrSQLCon As String = "data source=MAR-RGPR;initial catalog=DATA125;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st1rpk2qt5z"
        //'Friend Const StrSQLCON As String = "data source=MAIN01;initial catalog=DATA125;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt0z"

        //'Consorcio CastilloJ - cj.dynip.com
        //'Friend Const StrSQLCon As String = "data source=CASTILLOJ-MAR;initial catalog=DATA126;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st1rpk2qt6z"

        //'Consorcio Grandes Ligas - grandesligas.dynip.com
        //'Friend Const StrSQLCon As String = "data Source=GLigas-MAR;initial catalog=DATA127;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st1rpk2qt7z"

        //'Consorcio HS2013 - hs2013.dynip.com:7500 - 192.168.50.254
        //'Friend Const StrSQLCon As String = "data Source=HS2013-MAR;initial catalog=DATA128;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st1rpk2qt8z"

        //'Consorcio Fausto Jimenez - faustojimenez.dynip.com
        //'Friend Const StrSQLCon As String = "data Source=FJimenez-MAR;initial catalog=DATA129;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st1rpk2qt9z"

        //'Consorcio Pablo Jimenez - pablojimenez.dynip.com
        //'Friend Const StrSQLCon As String = "data Source=PJimenez-MAR;initial catalog=DATA130;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st1rpk3qt0z"

        //'Consorcio Nicolas Marling - nmarling.dynip.com
        //'Friend Const StrSQLCon As String = "data Source=NMarling-MAR;initial catalog=DATA131;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st1rpk3qt1z"
        //'Friend Const StrSQLCON As String = "data source=MAIN01;initial catalog=DATA131;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt0z"

        //'Consorcio AA - aa.dynip.com
        //'Friend Const StrSQLCon As String = "data Source=AA-MAR;initial catalog=DATA132;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st1rpk3qt2z"

        //'Consorcio Mayra - mayra.dynip.com
        //'Friend Const StrSQLCon As String = "data Source=Mayra-MAR;initial catalog=DATA133;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st1rpk3qt3z"

        //'Consorcio Starling - starling.dynip.com
        //'Friend Const StrSQLCon As String = "data Source=STARLING-MAR;initial catalog=DATA134;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st1rpk3qt4z"

        //'Consorcio Mi Patria - 172.31.0.1 - (Luis Garcia)
        //'Friend Const StrSQLCon As String = "data Source=NCR;initial catalog=DATA135;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st1rpk3qt5z"

        //'Consorcio 0172-Gutierrez - gutierrez.dynip.com
        //'Friend Const StrSQLCon As String = "data Source=GUTIERREZ-MAR;initial catalog=DATA136;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st1rpk3qt6z"

        //'Consorcio 0173-DKD DEKADA - dkd.dynip.com
        //'Friend Const StrSQLCon As String = "data Source=DEKADA-MAR;initial catalog=DATA137;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st1rpk3qt7z"

        //'Consorcio El Valle - NY - elvalle.dynip.com (Lorenzo)
        //'Friend Const StrSQLCon As String = "data Source=ELVALLE-MAR;initial catalog=DATA138;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st1rpk3qt8z"

        //'Kiro - kiro.dynip.com
        //'Friend Const StrSQLCon As String = "data source=MAR-KIRO;initial catalog=DATA139;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st1rpk3qt9z"

        //'Miguelina - miguelina.dynip.com
        //'Friend Const StrSQLCon As String = "data source=MAR-MIGUE;initial catalog=DATA140;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st1rpk4qt0z"
        //'Friend Const StrSQLCON As String = "data source=MAIN01;initial catalog=DATA140;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt0z"
        //'Private AllowedHosts As String() = {"miguelina.dynip.com", "miguelina.dynip.com:80"}

        //'Jeremias Jimenez - jjimenez.dynip.com
        //'Friend Const StrSQLCon As String = "data source=MAR-JJNEZ;initial catalog=DATA141;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st1rpk4qt1z"
        //'Friend Const StrSQLCon As String = "data source=MAIN01;initial catalog=DATA141;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt0z"
        //'Private AllowedHosts As String() = {"bancajimenez.ddns.net", "bancajimenez.ddns.net:80", "jjimenez.dynip.com", "jjimenez.dynip.com:80"}

        //'FyC Puerto Rico - fc.dynip.com
        //'Friend Const StrSQLCon As String = "data source=MAR-FC;initial catalog=DATA142;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st1rpk4qt2z"

        //'Mega Puerto Rico - mega.dynip.com
        //'Friend Const StrSQLCon As String = "data source=MAR-Mega;initial catalog=DATA143;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st1rpk4qt3z"

        //'Consorcio Los Morochos - morochos.dynip.com
        //'Friend Const StrSQLCON As String = "data source=MAIN01;initial catalog=DATA144;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt0z"

        //'Consorcio Diamond
        //'Friend Const StrSQLCON As String = "data source=MAIN01;initial catalog=DATA145;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt0z"

        //'Consorcio La Dinamica - ladinamica.dynip.com - Version 2014-05-01
        //'Friend Const StrSQLCON As String = "data source=DINAMIC-MAR;initial catalog=DATA146;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st1rpk4qt6z"
        //'Private AllowedHosts As String() = {"ladinamica.dynip.com", "ladinamica.dynip.com:80", "ladinamica.ddns.net", "ladinamica.ddns.net:80"}

        //'Consorcio Edward Rodriguez
        //'Friend Const StrSQLCON As String = "data source=MAIN01;initial catalog=DATA147;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt0z"

        //'Consorcio SH2 - sh2.dynip.com:7600 - Version 2014-05-01
        //'Friend Const StrSQLCon As String = "data source=MAR-SH2;initial catalog=DATA148;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st1rpk4qt8z"

        //'CONSORCIO DON MIGUEL - donmiguel.dynip.com
        //'Friend Const StrSQLCON As String = "data source=MAIN01;initial catalog=DATA149;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt0z"

        //'CONSORCIO PAGA MAS - pagamas.ddns.net
        //'Friend Const StrSQLCON As String = "data source=MAIN01;initial catalog=DATA150;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt0z"

        //'CONSORCIO LA MORENA - lamorena.ddns.net
        //'Friend Const StrSQLCON As String = "data source=MAIN01;initial catalog=DATA151;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt0z"

        //'CONSORCIO Metro Sport - metrosport.ddns.net
        //'Friend Const StrSQLCON As String = "data source=MAIN01;initial catalog=DATA152;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt0z"

        //'CONSORCIO Emmanuel Bautista - ebautista.ddns.net
        //'Friend Const StrSQLCON As String = "data source=MAIN01;initial catalog=DATA153;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt0z"

        //'CONSORCIO DIOGENES - diogenes.ddns.net
        //'Friend Const StrSQLCON As String = "data source=MAR-Diogenes;initial catalog=DATA154;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st1rpk5qt4z"

        //'CONSORCIO MM - mellizos ya no pueden tener mm.ddns.net
        //'Friend Const StrSQLCON As String = "data source=MAIN01;initial catalog=DATA155;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt0z"

        //'CONSORCIO R - bancaR.ddns.net
        //'Friend Const StrSQLCON As String = "data source=MAIN01;initial catalog=DATA156;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt0z"
        //'Private AllowedHosts As String() = {"bancar.ddns.net", "bancar.ddns.net:80"}

        //'CONSORCIO BISONO - bisono.ddns.net
        //'Friend Const StrSQLCON As String = "data source=MAIN01;initial catalog=DATA157;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt0z"

        //'Consorcio SH NY - shny.dynip.com:7700 - Version 2014-05-01
        //'Friend Const StrSQLCon As String = "data source=MAR-SHNY;initial catalog=DATA158;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st1rpk5qt8z"

        //'CONSORCIO MK - bancamk.ddns.net
        //'Friend Const StrSQLCON As String = "data source=MAIN01;initial catalog=DATA159;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt0z"

        //'CONSORCIO Rivas Rijo - rivasrijo.ddns.net
        //'Friend Const StrSQLCON As String = "data source=MAIN01;initial catalog=DATA160;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt0z"

        //'Banca Jimmy - bancajimmy.ddns.net
        //'Friend Const StrSQLCON As String = "data source=MAIN01;initial catalog=DATA161;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt0z"

        //'Banca Canelo - canelo.ddns.net
        //'Friend Const StrSQLCON As String = "data source=MAIN01;initial catalog=DATA162;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt0z"

        //'Banca La Magia - lamagia.ddns.net
        //'Friend Const StrSQLCon As String = "data source=MAR-MAGIA;initial catalog=DATA163;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st1rpk6qt3z"

        //'Banca Madrid - madrid.ddns.net - Version 2014-04-01
        //'Friend Const StrSQLCON As String = "data source=MAIN01;initial catalog=DATA164;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt0z"

        //'Banca DL4 - dl4.ddns.net - Version 2014-04-01
        //'Friend Const StrSQLCON As String = "data source=MAIN01;initial catalog=DATA165;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt0z"

        //'Banca MELO2 - melo.ddns.net - Version 2014-05-01
        //'Friend Const StrSQLCON As String = "data source=MAIN01;initial catalog=DATA166;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt0z"

        //'Banca Fulbio - fulbio.ddns.net - Version 2014-05-01
        //'Friend Const StrSQLCON As String = "data source=MAIN01;initial catalog=DATA167;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt0z"

        //'Consorcio Mi Suerte JC - misuerte.ddns.net - Version 2014-06-20
        //'Friend Const StrSQLCon As String = "data source=MAIN01;initial catalog=DATA168;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt0z"
        //'Private AllowedHosts As String() = {"misuerte.ddns.net", "misuerte.ddns.net:80"}

        //'LA REYNA - Version 2014-06-20
        //'Friend Const StrSQLCon As String = "data source=MAIN01;initial catalog=DATA169;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt0z"
        //'Private AllowedHosts As String() = {"lareyna.ddns.net", "lareyna.ddns.net:80"}

        //'BANCA GARCIA - Version 2014-06-20
        //'Friend Const StrSQLCon As String = "data source=MAIN01;initial catalog=DATA170;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt0z"
        //'Private AllowedHosts As String() = {"bancagarcia.ddns.net", "bancagarcia.ddns.net:80"}

        //'BANCA LB Michel Ramirez - Version 2014-06-20
        //'Friend Const StrSQLCon As String = "data source=MAIN01;initial catalog=DATA171;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt0z"
        //'Private AllowedHosts As String() = {"bancalb.ddns.net", "bancalb.ddns.net:80"}

        //'BANCA RULETA SPORT - Version 2014-06-20
        //'Friend Const StrSQLCon As String = "data source=MAIN01;initial catalog=DATA172;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt0z"
        //'Private AllowedHosts As String() = {"ruleta.ddns.net", "ruleta.ddns.net:80"}

        //'BANCA BAEZ - Version 2014-06-20
        //'Friend Const StrSQLCon As String = "data source=MAIN01;initial catalog=DATA173;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt0z"
        //'Private AllowedHosts As String() = {"bancabaez.ddns.net", "bancabaez.ddns.net:80"}

        //'BANCA RODRIGUEZ - Version 2014-06-20
        //'Friend Const StrSQLCon As String = "data source=MAIN01;initial catalog=DATA174;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt0z"
        //'Private AllowedHosts As String() = {"hnosrodriguez.ddns.net", "hnosrodriguez.ddns.net:80"}

        //'BANCA WR - Version 2014-06-20
        //'Friend Const StrSQLCon As String = "data source=MAIN01;initial catalog=DATA175;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt0z"
        //'Private AllowedHosts As String() = {"bancawr.ddns.net", "bancawr.ddns.net:80"}

        //'BANCA MEXICO - Version 2014-09-30
        //'Friend Const StrSQLCon As String = "data source=MAIN01;initial catalog=DATA176;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt0z"
        //'Private AllowedHosts As String() = {"bancamexico.ddns.net", "bancamexico.ddns.net:80"}

        //'BANCA SOÑADORA SPEAIN - Version 2014-09-30
        //'Friend Const StrSQLCon As String = "data source=MAIN01;initial catalog=DATA177;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt0z"
        //'Private AllowedHosts As String() = {"sonadora.ddns.net", "sonadora.ddns.net:80"}

        //'BANCA BAMBI - Version 2014-09-30
        //'Friend Const StrSQLCon As String = "data source=MAIN01;initial catalog=DATA178;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt0z"
        //'Private AllowedHosts As String() = {"bancabambi.ddns.net", "bancabambi.ddns.net:80"}

        //'BANCA RONALD SPORT - Version 2014-09-30
        //'Friend Const StrSQLCon As String = "data source=MAIN01;initial catalog=DATA179;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt0z"
        //'Private AllowedHosts As String() = {"ronaldsport.ddns.net", "ronaldsport.ddns.net:80"}

        //'Consorcio JEY - Version 2014-10-30
        //'Friend Const StrSQLCon As String = "data source=MAIN01;initial catalog=DATA180;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt0z"
        //'Private AllowedHosts As String() = {"jey.ddns.net", "jey.ddns.net:80"}

        //'Consorcio La Via - Version 2014-10-30
        //'Friend Const StrSQLCon As String = "data source=MAIN01;initial catalog=DATA181;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt0z"
        //'Private AllowedHosts As String() = {"lavia.ddns.net", "lavia.ddns.net:80"}

        //'Consorcio La Mole - Version 2014-10-30
        //'Friend Const StrSQLCon As String = "data source=MAIN01;initial catalog=DATA182;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt0z"
        //'Private AllowedHosts As String() = {"lamole.ddns.net", "lamole.ddns.net:80"}

        //'Consorcio Resuelve - Version 2014-10-30
        //'Friend Const StrSQLCon As String = "data source=MAIN01;initial catalog=DATA183;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt0z"
        //'Private AllowedHosts As String() = {"resuelve.ddns.net", "resuelve.ddns.net:80"}

        //'Consorcio Resuelve - Version 2014-10-30
        //'Friend Const StrSQLCon As String = "data source=MAIN01;initial catalog=DATA184;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt0z"
        //'Private AllowedHosts As String() = {"lafortuna.ddns.net", "lafortuna.ddns.net:80"}

        //'Consorcio Charly - Version 2014-10-30
        //'Friend Const StrSQLCon As String = "data source=MAIN01;initial catalog=DATA185;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt0z"
        //'Private AllowedHosts As String() = {"bancacharly.ddns.net", "bancacharly.ddns.net:80"}

        //'Consorcio Gonzalez - Version 2014-10-30
        //'Friend Const StrSQLCon As String = "data source=MAIN01;initial catalog=DATA186;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt0z"
        //'Private AllowedHosts As String() = {"gonzalez.ddns.net", "gonzalez.ddns.net:80"}

        //'Consorcio Las BBB - Version 2014-10-30
        //'Friend Const StrSQLCon As String = "data source=MAIN01;initial catalog=DATA187;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt0z"
        //'Private AllowedHosts As String() = {"lasbbb.ddns.net", "lasbbb.ddns.net:80"}

        //'Consorcio LOTTERY - Version 2014-10-30
        //'Friend Const StrSQLCon As String = "data source=MAIN01;initial catalog=DATA188;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt0z"
        //'Private AllowedHosts As String() = {"lottery.ddns.net", "lottery.ddns.net:80"}

        //'Consorcio Leo Castillo - Version 2014-10-30
        //'Friend Const StrSQLCon As String = "data source=MAR-LEO;initial catalog=DATA189;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st1rpk8qt9z"
        //'Private AllowedHosts As String() = {"leocastillo.ddns.net", "leocastillo.ddns.net:80"}

        //'Consorcio Lucky Espana - Version 2014-10-30
        //'Friend Const StrSQLCon As String = "data source=MAIN01;initial catalog=DATA190;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt0z"
        //'Private AllowedHosts As String() = {"bancald.ddns.net", "bancald.ddns.net:80"}

        //'Consorcio Capellan - Version 2014-10-30
        //'Friend Const StrSQLCon As String = "data source=MAIN01;initial catalog=DATA191;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt0z"
        //'Private AllowedHosts As String() = {"capellan.ddns.net", "capellan.ddns.net:80"}

        //'Consorcio Verdugo - Version 2014-10-30
        //'Friend Const StrSQLCon As String = "data source=VERDUGO-MAR;initial catalog=DATA192;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st1rpk9qt2z"
        //'Private AllowedHosts As String() = {"verdugo.ddns.net", "verdugo.ddns.net:80"}

        //'Consorcio Tu Futuro - Version 2014-10-30
        //'Friend Const StrSQLCon As String = "data source=MAIN01;initial catalog=DATA193;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt0z"
        //'Private AllowedHosts As String() = {"tufuturo.ddns.net", "tufuturo.ddns.net:80"}

        //'Consorcio Mr Nice Guy - Version 2015-04-11
        //'Friend Const StrSQLCon As String = "data source=MAIN01;initial catalog=DATA194;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt0z"
        //'Private AllowedHosts As String() = {"mrniceguy.ddns.net", "mrniceguy.ddns.net:80"}

        //'Consorcio Fenix - Version 2015-04-11
        //'Friend Const StrSQLCon As String = "data source=MAIN01;initial catalog=DATA195;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt0z"
        //'Private AllowedHosts As String() = {"bancafenix.ddns.net", "bancafenix.ddns.net:80"}

        //'Consorcio KIKO - Version 2015-04-21
        //'Friend Const StrSQLCon As String = "data source=MAIN01;initial catalog=DATA196;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt0z"
        //'Private AllowedHosts As String() = {"bancakiko.ddns.net", "bancakiko.ddns.net:80"}

        //'Consorcio BM - Version 2015-04-21
        //'Friend Const StrSQLCon As String = "data source=MAIN01;initial catalog=DATA197;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt0z"
        //'Private AllowedHosts As String() = {"bancabm.ddns.net", "bancabm.ddns.net:80"}

        //'Consorcio Llueve - Version 2015-04-21
        //'Friend Const StrSQLCon As String = "data source=MAIN01;initial catalog=DATA198;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt0z"
        //'Private AllowedHosts As String() = {"llueve.ddns.net", "llueve.ddns.net:80"}

        //'Consorcio Llueve - Version 2015-04-21
        //'Friend Const StrSQLCon As String = "data source=MAIN01;initial catalog=DATA199;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt0z"
        //'Private AllowedHosts As String() = {"jabao.ddns.net", "jabao.ddns.net:80"}

        //'Consorcio Freddy Fortuna - Version 2015-04-21
        //'Friend Const StrSQLCon As String = "data source=MAIN01;initial catalog=DATA200;persist security info=True;Min Pool Size=10;user id=sa2;pwd=st0rpk0qt0z"
        //'Private AllowedHosts As String() = {"ffortuna.ddns.net", "ffortuna.ddns.net:80"}

        //'Private gCommandTimeOut As Integer = 210

    //}
}
