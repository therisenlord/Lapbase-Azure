<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt">

<xsl:template match="/">
    <html>
    <body>
      
      <table style="width:97%" cellpadding = "0" cellspacing = "0" border="0" id ="tblDoctorXSLT" class="testNameTable">
        <xsl:for-each select ="/dsSchema/tblHospital">
        <tr >
          <xsl:if test='position() mod 2 = 1'>
            <xsl:attribute name="class">row01</xsl:attribute>
          </xsl:if>
          <xsl:if test='position() mod 2 != 1'>
            <xsl:attribute name="class">row02</xsl:attribute>
          </xsl:if>
          <xsl:attribute name ="onclick">
            javascript:tblHospitalXSLT_onclick('<xsl:value-of select="Hospital_Id"/>', '<xsl:value-of select="Hospital_Name"/>', '<xsl:value-of select="HospitalBoldCode"/>', '<xsl:value-of select="Region_Id"/>');
          </xsl:attribute>
          <td style="width :5%"  ><xsl:value-of select="Hospital_Id"/></td>
          <td style="width :40%" >
            <xsl:value-of select="Hospital_Name"/>
          </td>
          <td style="width :52%" />
        </tr>
        </xsl:for-each>
      </table>
    </body>
    </html>
</xsl:template>

</xsl:stylesheet> 

