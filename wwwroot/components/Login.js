const Login = async (Email, PasswordHash, event) => {

    if (!$("#loginModalForm").valid())
        return;

    const data = {
        Email: Email.value,
        PasswordHash: PasswordHash.value
    };

    try {
        const results = await fetchWithParams("User/Login", data, "POST");
        console.log(results);
    } catch (error) {
        console.error('Error:', error);
    }
}

export default {
    Login
}