<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt">

<xsl:template match="/">
    <html>
    <body>
    <!--
        This is an XSLT template file. Fill in this area with the
        XSL elements which will transform your XML to XHTML.
    -->
      <table style="width:99%" cellpadding = "0" cellspacing = "0" border="0"  class="testNameTable">
        <xsl:for-each select="dsSchema/tblPatientOperation">
          <tr>
			  <td>
				  <xsl:if test='position() mod 2 = 1'>
					  <xsl:attribute name="class">row01</xsl:attribute>
				  </xsl:if>
				  <xsl:if test='position() mod 2 != 1'>
					  <xsl:attribute name="class">row02</xsl:attribute>
				  </xsl:if>
				  <table style="width:100%" cellpadding = "0" cellspacing = "0" border="0">
					  <tr>
						  <td style="width:75px">
							  <xsl:value-of select="strDateOperation"/>
						  </td>
						  <td style="width:160px">
							  <xsl:value-of select="SugeryDesc"/>
						  </td>
						  <td style="width:150px">
							  <xsl:value-of select="ApproachDesc"/>
						  </td>

						  <td style="width:180px">
							  <xsl:value-of select="SurgeonName"/>
						  </td>
						  <td style="width:180px">
							  <xsl:value-of select="HospitalName"/>
						  </td>
              <td style="width:120px">
                <xsl:call-template name ="fnLoadNumber">
                  <xsl:with-param name="numValue" select ="Weight"/>
                </xsl:call-template>
              </td>
						  <!-- <td style="width:130px; display:none">
							  <xsl:value-of select="DaysInHospital"/>
						  </td> -->
					  </tr>
				  </table>
			  </td>
          </tr>
        </xsl:for-each>
      </table>
    </body>
    </html>
</xsl:template>
<xsl:template name = "fnLoadNumber">
  <xsl:param name ="numValue" ></xsl:param>
  <xsl:choose >
    <xsl:when test = "numValue=round($numValue)">
      <xsl:value-of select="round($numValue)"/>
    </xsl:when>
    <xsl:otherwise>
      <xsl:value-of select="format-number($numValue, '###.#')"/>
    </xsl:otherwise>
  </xsl:choose>
</xsl:template>
</xsl:stylesheet> 

