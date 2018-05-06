<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt">

<xsl:template match="/">
    <html>
    <body>
      
      <table style="width:97%" cellpadding = "0" cellspacing = "0" border="0" id ="tblActionLogXSLT" class="testNameTable">
        <xsl:for-each select ="/dsSchema/tblActionLog">
        <tr >
          <xsl:if test='position() mod 2 = 1'>
            <xsl:attribute name="class">row01</xsl:attribute>
          </xsl:if>
          <xsl:if test='position() mod 2 != 1'>
            <xsl:attribute name="class">row02</xsl:attribute>
          </xsl:if>
          <td style="width :8%"  >
            <xsl:value-of select="UserID"/>
          </td>
          <td style="width :8%"  >
            <xsl:value-of select="Page"/>
          </td>
          <td style="width :5%"  >
            <xsl:value-of select="ActionDesc"/>
          </td>
          <td style="width :24%"  >
            <xsl:value-of select="ActionDetail"/>
          </td>
          <td style="width :13%"  >
            <xsl:value-of select="CustomPatientID"/>
          </td>
          <td style="width :14%"  >
            <xsl:value-of select="PatientName"/>
          </td>
          <td style="width :8%"  >
            <xsl:value-of select="RecordID"/>
          </td>
          <td style="width :12%"  >
            <xsl:value-of select="DateTimeDesc"/>
          </td>
        </tr>
        </xsl:for-each>
      </table>
    </body>
    </html>
</xsl:template>

</xsl:stylesheet> 

