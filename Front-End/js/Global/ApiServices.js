const base_Url = "http://localhost:5279/api/v1/";

export async function getData(endpointUrl)
{
    try
    {
       const response = await fetch(`${base_Url}${endpointUrl}`, {
        method: "GET",
        headers: {
            "Accept": "application/json"
        }
        });
        const data = await response.json();
        return data;
    }
    catch(error)
    {
        throw new Error(error.message || "No se pudo conectar con el servidor");
    }
}

export async function postData(endpointUrl, body) {
    try {
        const response = await fetch(`${base_Url}${endpointUrl}`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(body)
        });

        let data = null;

        const contentType = response.headers.get("content-type");

        if (contentType && contentType.includes("application/json")) {
            data = await response.json();
        }

        if (!response.ok) {
            throw new Error(data?.message || "Error en la solicitud");
        }

        return data;

    } catch (error) {
        throw new Error(error.message || "No se pudo conectar con el servidor");
    }
}

export async function deleteData(endpointUrl)
{
    try
    {
       const response = await fetch(`${base_Url}${endpointUrl}`, {
        method: "DELETE",
        headers: {
            "Accept": "application/json"
        }
        });
        const data = await response.json();
        return data;
    }
    catch(error)
    {
        throw new Error(error.message || "No se pudo conectar con el servidor");
    }
}

export async function putData(endpointUrl, body) {
    try {
        const response = await fetch(`${base_Url}${endpointUrl}`, {
            method: "PUT",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(body)
        });

        let data = null;
        const text = await response.text();
        data = text ? JSON.parse(text) : null;

        if (!response.ok) {
            throw new Error(
                data?.message ||
                data?.Message ||
                `Error HTTP ${response.status}`
            );
        }

        return data;

    } catch (error) {
        throw error; 
    }
}

export async function getDataQueryParams(endpointUrl, params = {})
{
  try {

    const query = new URLSearchParams(params).toString();

    const response = await fetch(
      `${base_Url}${endpointUrl}${query ? `?${query}` : ""}`,
      {
        method: "GET",
        headers: { "Accept": "*/*" }
      }
    );

    const data = await response.json();

    if (!response.ok) {
      throw new Error(data?.message || "Error en la solicitud");
    }

    return data;

  } catch (error) {
    throw new Error(error.message || "No se pudo conectar con el servidor");
  }
}

export async function putDataWithQueryParams(endpointUrl, params = {}) {
  const query = new URLSearchParams(params).toString();

  const url = `${base_Url}${endpointUrl}?${query}`;

  try {
    const response = await fetch(url, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json"
      }
    });

    const text = await response.text();
    const data = text ? JSON.parse(text) : null;

    if (!response.ok) {
      throw new Error(
        data?.message ||
        data?.Message ||
        `Error HTTP ${response.status}`
      );
    }

    return data;

  } catch (error) {
    throw error;
  }
}