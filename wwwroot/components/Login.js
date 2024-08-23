const loginOption = defineAsyncComponent(async () => {
    return {
        template: await loadTemplate("Template/_Login"),
        props: {
            onLoggedIn: {
                type: Function,
                required: true
            }
        },
        setup() {
            const email = ref('');
            const passwordHash = ref('');
            const errorMsg = ref('');

            const clear = () => {
                email.value = '';
                passwordHash.value = '';
                errorMsg.value = '';
            }

            const showModel = async () => {
                clear();
                $("#loginModal").modal("show");
                $.validator.unobtrusive.parse($("#loginModal"));
            }

            const login = async () => {
                if (!$("#loginModalForm").valid())
                    return;

                const data = {
                    Email: email.value,
                    PasswordHash: passwordHash.value
                };

                try {
                    const results = await fetchWithParams("User/Login", data, "POST");

                    if (results && results.state) {
                        clear();
                        $("#loginModal").modal("hide");
                        props.onLoggedIn();
                    } else {
                        errorMsg.value = results.errorMsg;
                        passwordHash.value = '';
                    }

                } catch (error) {
                    console.error('Error:', error);
                }
            };

            return {
                email,
                passwordHash,
                login,
                showModel,
                errorMsg
            };
        }
    }
});

export default loginOption