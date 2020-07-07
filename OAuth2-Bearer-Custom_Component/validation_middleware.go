package validation

import (
	"fmt"
	"strings"

	"github.com/dapr/components-contrib/middleware"
	"github.com/dapr/dapr/pkg/logger"
	"github.com/dgrijalva/jwt-go"
	"github.com/fasthttp-contrib/sessions"
	"github.com/valyala/fasthttp"
)

type validationMiddlewareMetadata struct {
}

func NewValidationMiddleware(logger logger.Logger) *Middleware {
	return &Middleware{logger: logger}
}

type Middleware struct {
	logger logger.Logger
}

const (
	bearerPrefix       = "bearer "
	bearerPrefixLength = len(bearerPrefix)
)

func (m *Middleware) GetHandler(metadata middleware.Metadata) (func(h fasthttp.RequestHandler) fasthttp.RequestHandler, error) {
	return func(h fasthttp.RequestHandler) fasthttp.RequestHandler {
		return func(ctx *fasthttp.RequestCtx) {
			session := sessions.StartFasthttp(ctx)
			authHeader := string(ctx.Request.Header.Peek("Authorization"))
			if authHeader == "" || !strings.HasPrefix(strings.ToLower(authHeader), bearerPrefix) {
				ctx.Error(fasthttp.StatusMessage(fasthttp.StatusUnauthorized), fasthttp.StatusUnauthorized)
				return
			}
			rawToken := authHeader[bearerPrefixLength:]
			parts := strings.Split(rawToken, ".")
			if len(parts) != 3 {
				ctx.Error(fasthttp.StatusMessage(fasthttp.StatusUnauthorized), fasthttp.StatusUnauthorized)
				return
			}
			token, _, err := new(jwt.Parser).ParseUnverified(rawToken, jwt.MapClaims{})
			if err != nil {
				fmt.Println(err)
			}
			if claims, ok := token.Claims.(jwt.MapClaims); ok {
				for key, val := range claims {
					fmt.Printf("%v : %v\n", key, val)
				}
			} else {
				fmt.Println(err)
			}
			session.Set("Authorization", authHeader)
			ctx.Request.Header.Add("Authorization", authHeader)
			h(ctx)
			return
		}

	}, nil

}
