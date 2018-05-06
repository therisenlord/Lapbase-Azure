<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt">

<xsl:template match="/">
    <html>
    <body>
      
      <table style="width:97%" cellpadding = "0" cellspacing = "0" border="0" id ="tblDoctorXSLT" class="testNameTable">
        <xsl:for-each select ="/dsSchema/tblDoctors">
        <tr >
          <xsl:if test='position() mod 2 = 1'>
            <xsl:attribute name="class">row01</xsl:attribute>
          </xsl:if>
          <xsl:if test='position() mod 2 != 1'>
            <xsl:attribute name="class">row02</xsl:attribute>
          </xsl:if>
          <xsl:attribute name ="onclick">javascript:tblDoctorXSLT_onclick(<xsl:value-of select="DoctorID"/>);</xsl:attribute>
          <td style="width :4%"  ><xsl:value-of select="position()"/></td>
          <td style="width :13%" >
            <xsl:value-of select="Doctor_Name"/>
          </td>
          <td style="width :11%" >
            <xsl:value-of select="Speciality"/>
          </td>
          <td style="width :10%" >
            <xsl:value-of select="Degrees"/>
          </td>
          <td style="width :25%" >
            <xsl:value-of select="PrefSurgeryTypeDesc"/>
          </td>
          <td style="width :15%" >
            <xsl:value-of select="PrefApproachDesc"/>
          </td>
          <td style="width :19%" >
            <xsl:value-of select="PrefCategoryDesc"/>
          </td>
        </tr>
        </xsl:for-each>
      </table>
    </body>
    </html>
</xsl:template>

</xsl:stylesheet> 

