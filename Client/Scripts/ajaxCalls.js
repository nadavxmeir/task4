function ajaxCall(method, api, data, successCB, errorCB) {
  let fullUrl;
  if (api.startsWith("/api")) {
    if (
      window.location.hostname === "localhost" ||
      window.location.hostname === "127.0.0.1"
    ) {
      // סביבת פיתוח (Development)
      fullUrl = "https://localhost:7006" + api;
      console.log("Running in Development Mode: ", fullUrl);
    } else {
      // סביבת ייצור (Production)
      fullUrl = "https://proj.ruppin.ac.il/cgroup31/test2/tar1" + api;
      console.log("Running in Production Mode:", fullUrl);
    }
  } else {
    fullUrl = api;
    console.log(api);
  }
  $.ajax({
    type: method,
    url: fullUrl,
    data: data,
    cache: false,
    contentType: "application/json",
    success: successCB,
    error: errorCB,
  });
}

function formatDate(dateStr) {
  if (!dateStr) return "N/A";
  const d = new Date(dateStr);
  if (isNaN(d)) return dateStr.split(" ")[0] || "N/A"; // במקרה של שגיאת המרה
  // מחזיר פורמט בדיוק כמו ב-JSON המקורי: "Aug 21, 2012"
  return d.toLocaleDateString("en-US", {
    month: "short",
    day: "numeric",
    year: "numeric",
  });
}
